using System;
using System.Drawing;
using System.Threading;
using System.Collections;
using AgentServerObjects;
using AgentObjects;


namespace Merlin {
    // Thread safeness is to be added to that class
    public class EventsMgr {
        //[System.Reflection.MethodImplAttributes.Synchronized(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static void AddEvent(int dwRequestID) {
            Events.Add(dwRequestID, new AutoResetEvent(false));
        }
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static void SignalEvent(int dwRequestID) {
            AutoResetEvent ev = (AutoResetEvent)Events[dwRequestID];
            if (null != ev) {
                ev.Set();
            }
        }
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        public static void WaitAndRemove(int dwRequestID) {
            AutoResetEvent ev = (AutoResetEvent)Events[dwRequestID];
            if (null != ev) {
                ev.WaitOne();
                Events.Remove(dwRequestID);
            }
        }

        private static SortedList Events = new SortedList();
    }

    public class AgentServerType {
        private const int FAILURE = -1;
        private const int SUCCESS = 100;

        private IAgentEx m_SrvEx = null;
        public IAgentEx Srv {
            get {
                return m_SrvEx;
            }
        }

        private AgentNotifySink m_Sink;
        public AgentNotifySink Sink {
            get {
                return m_Sink;
            }
        }

        public void Initialize() {
            if (m_SrvEx != null) {
                return; //Already initialized
            }
            Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            try {
                AgentServer AgSrv = new AgentServer();
                // The following cast does the QueryInterface to fetch IAgentEx interface from the IAgent interface, directly supported by the object
                m_SrvEx = (IAgentEx)AgSrv;

                m_Sink = new AgentNotifySink();
                m_Sink.Register(m_SrvEx);
            } catch (Exception e) {
                AgentServerStartupException ex = new AgentServerStartupException("Agent Server could not be started", e);
                Console.WriteLine(ex);
                throw ex;
            }
        }

        public void Shutdown() {
            // Shutdown sequence
            //Console.WriteLine("Shutdown in progress...");
            if (m_Sink != null) {
                m_Sink.Unregister(m_SrvEx);
                m_Sink = null;
            }
        }
    }

    public delegate void MovedDelegate(Point p);
    public delegate void RightClickedDelegate(Point p);
    public delegate void OnCommandDelegate(int CommandID);
    public delegate void OnPropertyChangeDelegate();

    public class AgentNotifySink : IAgentNotifySinkEx {
        private int m_nNotifySinkID = -1;

        public void Register(IAgentEx SrvEx) {
            SrvEx.Register(this, out m_nNotifySinkID);
        }

        public void Unregister(IAgentEx SrvEx) {
            if (m_nNotifySinkID != -1) {
                SrvEx.Unregister(m_nNotifySinkID);
            }
        }

        // IAgentNotifySinkEx implementation below
        public event OnCommandDelegate OnCommandEvent;
        public virtual void Command(Int32 dwCommandID, System.Object punkUserInput) {
            OnCommandEvent(dwCommandID);
        } // end of method IAgentNotifySinkEx::Command

        public virtual void
                ActivateInputState(Int32 dwCharID, Int32 bActivated) {
        } // end of method IAgentNotifySinkEx::ActivateInputState

        public virtual void Restart() {
        } // end of method IAgentNotifySinkEx::Restart

        public virtual void Shutdown() {
        } // end of method IAgentNotifySinkEx::Shutdown

        public virtual void VisibleState(Int32 dwCharID, Int32 bVisible, Int32 dwCause) {
        } // end of method IAgentNotifySinkEx::VisibleState

        public event RightClickedDelegate RightClickedEvent;
        public virtual void Click(Int32 dwCharID, Int16 fwKeys, Int32 x, Int32 y) {
            if ((fwKeys & AgentCharacter.MK_RBUTTON) != 0) {
                RightClickedEvent(new Point(x, y));
            }
        } // end of method IAgentNotifySinkEx::Click

        public virtual void DblClick(Int32 dwCharID, Int16 fwKeys, Int32 x, Int32 y) {
        } // end of method IAgentNotifySinkEx::DblClick

        public virtual void DragStart(Int32 dwCharID, Int16 fwKeys, Int32 x, Int32 y) {
        } // end of method IAgentNotifySinkEx::DragStart

        public virtual void DragComplete(Int32 dwCharID, Int16 fwKeys, Int32 x, Int32 y) {
        } // end of method IAgentNotifySinkEx::DragComplete

        public virtual void RequestStart(Int32 dwRequestID) {
        } // end of method IAgentNotifySinkEx::RequestStart

        public virtual void RequestComplete(Int32 dwRequestID, Int32 hrStatus) {
            Console.WriteLine("Request #{0} complete", dwRequestID);
            EventsMgr.SignalEvent(dwRequestID);
        } // end of method IAgentNotifySinkEx::RequestComplete

        public virtual void BookMark(Int32 dwBookMarkID) {
        } // end of method IAgentNotifySinkEx::BookMark

        public virtual void Idle(Int32 dwCharID, Int32 bStart) {
        } // end of method IAgentNotifySinkEx::Idle

        public event MovedDelegate MovedEvent;
        public virtual void Move(Int32 dwCharID, Int32 x, Int32 y, Int32 dwCause) {
            MovedEvent(new Point(x, y));
        } // end of method IAgentNotifySinkEx::Move

        public virtual void Size(Int32 dwCharID, Int32 lWidth, Int32 lHeight) {
        } // end of method IAgentNotifySinkEx::Size

        public virtual void BalloonVisibleState(Int32 dwCharID, Int32 bVisible) {
        } // end of method IAgentNotifySinkEx::BalloonVisibleState

        public virtual void HelpComplete(Int32 dwCharID, Int32 dwCommandID, Int32 dwCause) {
        } // end of method IAgentNotifySinkEx::HelpComplete

        public virtual void ListeningState(Int32 dwCharID, Int32 bListening, Int32 dwCause) {
        } // end of method IAgentNotifySinkEx::ListeningState

        public virtual void DefaultCharacterChange(System.String bszGUID) {
        } // end of method IAgentNotifySinkEx::DefaultCharacterChange

        public event OnPropertyChangeDelegate OnPropertyChangeEvent;
        public virtual void AgentPropertyChange() {
            OnPropertyChangeEvent();
        } // end of method IAgentNotifySinkEx::AgentPropertyChange

        public virtual void ActiveClientChange(Int32 dwCharID, Int32 lStatus) {
        } // end of method IAgentNotifySinkEx::ActiveClientChange
    } // AgentNotifySink

    public class AgentServerStartupException : Exception {
        public AgentServerStartupException(string message, Exception innerException)
            :
            base(message, innerException) {
        }
    }
}
