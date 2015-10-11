using System;
using System.Drawing;
using System.Collections; // For IEnumerator
using System.Runtime.InteropServices; // For Marshal
using AgentServerObjects;
using AgentObjects;


namespace Merlin {
    public class AgentCharacter {
        private static AgentServerType m_AgentSrv = null;
        private IAgentCharacterEx m_CharacterEx;
        private int m_nCharID = 0;

        public AgentNotifySink Sink {
            get {
                return m_AgentSrv.Sink;
            }
        }

        public static AgentCharacter LoadAgent(String CharacterFile) {
            if (m_AgentSrv == null) {
                m_AgentSrv = new AgentServerType();
            }
            m_AgentSrv.Initialize();
            AgentCharacter Agent = new AgentCharacter();
            Agent.Load(CharacterFile);
            return Agent;
        }

        private void Load(String CharacterFile) {
            try {
                int nReqID;
                // If the first parameter is null, then COM object gets VT_EMPTY variant, which means "default character"
                m_AgentSrv.Srv.Load(CharacterFile, out m_nCharID, out nReqID);
                m_AgentSrv.Srv.GetCharacterEx(m_nCharID, out m_CharacterEx);
                //m_CharacterEx.SetLanguageID(MAKELANGID(LANG_ENGLISH, SUBLANG_ENGLISH_US));

                // Show the character.  The first parameter tells Microsoft
                // Agent to show the character by playing an animation.
                Show(false);

                // Make the character speak
                //m_CharacterEx.Speak("Hello!", null, out nReqID);
                // If it is desired to wait until Agent finish talking, the following two lines would do that:
                // EventsMgr.AddEvent(nReqID);
                // EventsMgr.WaitAndRemove(nReqID);

                EnumerateAnimations();
            } catch (Exception e) {
                //e.Source = "Agent Server failed to load Agent Character file";
                AgentCharacterLoadingException ex = new AgentCharacterLoadingException("Agent Server failed to load Agent Character file", e);
                //Console.WriteLine(ex);
                throw ex;
            }
        }
        public void Unload() {
            if (m_AgentSrv.Srv != null && m_nCharID != 0) {
                m_AgentSrv.Srv.Unload(m_nCharID);
                m_nCharID = 0;
            }
            if (m_CharacterEx != null) {
                Marshal.ReleaseComObject(m_CharacterEx);
                m_CharacterEx = null;
            }
        }
        public static void Shutdown() {
            if (m_AgentSrv != null) {
                m_AgentSrv.Shutdown();
            }
        }

        public void StopAll() {
            m_CharacterEx.StopAll(STOP_TYPE_ALL);
        }
        public void Say(String str) {
            int nReqID;
            m_CharacterEx.Speak(str, null, out nReqID);
        }
        public void Play(String str) {
            int nReqID;
            m_CharacterEx.Play(str, out nReqID);
        }
        public void GestureAt(Point p) {
            int nReqID;
            m_CharacterEx.GestureAt((short)p.X, (short)p.Y, out nReqID);
        }

        public void Show(bool Fast) {
            int nReqID;
            m_CharacterEx.Show(Conv.ToInt32(Fast), out nReqID);
        }

        public void Hide(bool Fast) {
            int nReqID;
            m_CharacterEx.Hide(Conv.ToInt32(Fast), out nReqID);
        }

        public int VisibilityCause {
            get {
                int dwCause;
                m_CharacterEx.GetVisibilityCause(out dwCause);
                return dwCause;
            }
        }

        public bool AutoPopupMenu {
            get {
                int nAutoPopupMenu;
                m_CharacterEx.GetAutoPopupMenu(out nAutoPopupMenu);
                return nAutoPopupMenu != 0;
            }
            set {
                m_CharacterEx.SetAutoPopupMenu(Conv.ToInt32(value));
            }
        }
        public Point Position {
            get {
                int x, y;
                m_CharacterEx.GetPosition(out x, out y);
                return new Point(x, y);
            }
            set {
                int nReqID;
                m_CharacterEx.MoveTo((short)value.X, (short)value.Y, 1000, out nReqID);
            }
        }
        public bool SoundEffectsOn {
            get {
                int On;
                m_CharacterEx.GetSoundEffectsOn(out On);
                return Conv.ToBoolean(On);
            }
            set {
                m_CharacterEx.SetSoundEffectsOn(Conv.ToInt32(value));
            }
        }

        public bool HasOtherClients {
            get {
                int nOtherClients;
                m_CharacterEx.HasOtherClients(out nOtherClients);
                return nOtherClients > 0;
            }
        }
        public IAgentEx AgentEx {
            get {
                return m_AgentSrv.Srv;
            }
        }

        public IAgentCharacterEx CharacterEx {
            get {
                return m_CharacterEx;
            }
        }
        public IAgentBalloonEx Balloon {
            get {
                return (IAgentBalloonEx)m_CharacterEx;
            }
        }
        public IAgentCommands Commands {
            get {
                return (IAgentCommands)m_CharacterEx;
            }
        }
        public ArrayList EnumerateAnimations() {
            ArrayList res = new ArrayList();
            if (m_CharacterEx == null) {
                return res; // Return empty array
            }
            Object AnimNames;
            m_CharacterEx.GetAnimationNames(out AnimNames);

            // This code with Iterator is equivalent of the commented out paragraph below
            foreach (object o in new Iterator(AnimNames)) {
                if (null == o)
                    continue;
                String strAnim = o.ToString();
                Console.WriteLine(strAnim);
                res.Add(strAnim);
            }
            /*			
                        IEnumerator enumAnimNames = (IEnumerator) AnimNames;
                        while (enumAnimNames.MoveNext()) {
                            if (null == enumAnimNames.Current) continue;
                            String strAnim = enumAnimNames.Current.ToString();
                            Console.WriteLine(strAnim);
                            res.Add(strAnim);
                        }
            */
            res.Sort();
            return res;
        }
        internal const int NeverMoved = 0;
        internal const int UserMoved = 1;
        internal const int ProgramMoved = 2;
        internal const int OtherProgramMoved = 3;
        internal const int SystemMoved = 4;
        internal const int NeverShown = 0;
        internal const int UserHid = 1;
        internal const int UserShowed = 2;
        internal const int ProgramHid = 3;
        internal const int ProgramShowed = 4;
        internal const int OtherProgramHid = 5;
        internal const int OtherProgramShowed = 6;
        internal const int UserHidViaCharacterMenu = 7;
        internal const int UserHidViaTaskbarIcon = UserHid;
        internal const int CSHELPCAUSE_COMMAND = 1;
        internal const int CSHELPCAUSE_OTHERPROGRAM = 2;
        internal const int CSHELPCAUSE_OPENCOMMANDSWINDOW = 3;
        internal const int CSHELPCAUSE_CLOSECOMMANDSWINDOW = 4;
        internal const int CSHELPCAUSE_SHOWCHARACTER = 5;
        internal const int CSHELPCAUSE_HIDECHARACTER = 6;
        internal const int CSHELPCAUSE_CHARACTER = 7;
        internal const int ACTIVATE_NOTTOPMOST = 0;
        internal const int ACTIVATE_TOPMOST = 1;
        internal const int ACTIVATE_NOTACTIVE = 0;
        internal const int ACTIVATE_ACTIVE = 1;
        internal const int ACTIVATE_INPUTACTIVE = 2;
        internal const int PREPARE_ANIMATION = 0;
        internal const int PREPARE_STATE = 1;
        internal const int PREPARE_WAVE = 2;
        internal const int STOP_TYPE_PLAY = 0x1;
        internal const int STOP_TYPE_MOVE = 0x2;
        internal const int STOP_TYPE_SPEAK = 0x4;
        internal const int STOP_TYPE_PREPARE = 0x8;
        internal const int STOP_TYPE_NONQUEUEDPREPARE = 0x10;
        internal const int STOP_TYPE_VISIBLE = 0x20;
        internal const int STOP_TYPE_ALL = unchecked((int)0xffffffff);
        internal const int BALLOON_STYLE_BALLOON_ON = 0x1;
        internal const int BALLOON_STYLE_SIZETOTEXT = 0x2;
        internal const int BALLOON_STYLE_AUTOHIDE = 0x4;
        internal const int BALLOON_STYLE_AUTOPACE = 0x8;
        internal const int AUDIO_STATUS_AVAILABLE = 0;
        internal const int AUDIO_STATUS_NOAUDIO = 1;
        internal const int AUDIO_STATUS_CANTOPENAUDIO = 2;
        internal const int AUDIO_STATUS_USERSPEAKING = 3;
        internal const int AUDIO_STATUS_CHARACTERSPEAKING = 4;
        internal const int AUDIO_STATUS_SROVERRIDEABLE = 5;
        internal const int AUDIO_STATUS_ERROR = 6;
        internal const int LISTEN_STATUS_CANLISTEN = 0;
        internal const int LISTEN_STATUS_NOAUDIO = 1;
        internal const int LISTEN_STATUS_NOTACTIVE = 2;
        internal const int LISTEN_STATUS_CANTOPENAUDIO = 3;
        internal const int LISTEN_STATUS_COULDNTINITIALIZESPEECH = 4;
        internal const int LISTEN_STATUS_SPEECHDISABLED = 5;
        internal const int LISTEN_STATUS_ERROR = 6;
        internal const int MK_ICON = 0x1000;
        internal const int MK_LBUTTON = 0x0001;
        internal const int MK_RBUTTON = 0x0002;
        internal const int MK_SHIFT = 0x0004;
        internal const int MK_CONTROL = 0x0008;
        internal const int MK_MBUTTON = 0x0010;
        internal const int LSCOMPLETE_CAUSE_PROGRAMDISABLED = 1;
        internal const int LSCOMPLETE_CAUSE_PROGRAMTIMEDOUT = 2;
        internal const int LSCOMPLETE_CAUSE_USERTIMEDOUT = 3;
        internal const int LSCOMPLETE_CAUSE_USERRELEASEDKEY = 4;
        internal const int LSCOMPLETE_CAUSE_USERUTTERANCEENDED = 5;
        internal const int LSCOMPLETE_CAUSE_CLIENTDEACTIVATED = 6;
        internal const int LSCOMPLETE_CAUSE_DEFAULTCHARCHANGE = 7;
        internal const int LSCOMPLETE_CAUSE_USERDISABLED = 8;
    }

    public class Iterator {
        private IEnumerator m_Enumerator;
        public Iterator(object e) {
            m_Enumerator = (IEnumerator)e;
        }
        public IEnumerator GetEnumerator() {
            return m_Enumerator;
        }
    }
    public class AgentCharacterLoadingException : Exception {
        public AgentCharacterLoadingException(string message, Exception innerException)
            :
            base(message, innerException) {
        }
    }
    internal class Conv {
        internal static int ToInt32(bool b) {
            return b ? 1 : 0;
        }
        internal static bool ToBoolean(int n) {
            return (n != 0) ? true : false;
        }
    }
}
