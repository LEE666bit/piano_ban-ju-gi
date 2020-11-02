using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Web;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        const int _C = 0;
        const char C = 'C';
        const int _D = 2;
        const char D = 'D';
        const int _E = 4;
        const char E = 'E';
        const int _F = 5;
        const char F = 'F';
        const int _G = 7;
        const char G = 'G';
        const int _A = 9;
        const char A = 'A';
        const int _B = 11;
        const char B = 'B';

        const char sharp = '#'; const char flat = 'b';
        const String _7code = "7";
        const String m7code = "-7";
        const String M7code = "M7";
        const String dim7 = "dim7";

        //public Form1 newF;
        public int _4bit = 0; //4/4박자중 첫번째 박자로 초기화합니다.
        public int noteR, note3, note5, note7; //근음과 화음입니다.
        public int nowPlaying; //재생중인 마디의 번호입니다.
        public String nodeName; //재생중인 마디의 TextBox 이름입니다.
        SoundPlayer noteSound;

        public Form1(int playingNode)
        {
            int noteR = note3 = note5 = note7 = 0;
            nowPlaying = playingNode;
            noteSound = null;
            nodeName = "Node" + nowPlaying.ToString();
            InitializeComponent();
        }

        /*
        public void setNext(Form1 player)
        {
            newF = player;
        }

        public Form1 getNext()
        {
            return newF;
        }*/

        
        public void IDcode(String txt, int index, int ROOT) // 근음으로부터 화음을 쌓아올립니다.
        {
            if (txt.IndexOf(_7code) == index+1)
            {
                note3 = (ROOT + 4) % 12;
                note5 = (ROOT + 7) % 12;
                note7 = (ROOT + 10) % 12;
            }
            if (txt.IndexOf(m7code) == index+1)
            {
                note3 = (ROOT + 3) % 12;
                note5 = (ROOT + 7) % 12;
                note7 = (ROOT + 10) % 12;
            }
            if (txt.IndexOf(M7code) == index+1)
            {
                note3 = (ROOT + 4) % 12;
                note5 = (ROOT + 7) % 12;
                note7 = (ROOT + 11) % 12;
            }
            if (txt.IndexOf(dim7) == index+1)
            {
                note3 = (ROOT + 3) % 12;
                note5 = (ROOT + 6) % 12;
                note7 = (ROOT + 9) % 12;
            }
            return;
        }

        public int getCode(int ROOT) //textBox에서 코드를 구분합니다.
        {
            int i = 0;
            String code = (this.Controls["Node" + nowPlaying.ToString()] as TextBox).Text;
            char root = code[i];

            if (root == C)
            {
                ROOT = _C;
            }
            if (root==D)
            {
                ROOT = _D;
            }
            if (root==E)
            {
                ROOT = _E;
            }
            if (root==F)
            {
                ROOT = _F;
            }
            if (root==G)
            {
                ROOT = _G;
            }
            if (root==A)
            {
                ROOT = _A;
            }
            if (root==B)
            {
                ROOT = _B;
            }
            if (code[i + 1]==sharp)
            {
                i++;
                ROOT++;
            }
            if (code[i + 1]==flat)
            {
                i++;
                ROOT--;
            }
            IDcode(code, i, ROOT);
            return ROOT;
        }

        /*public int[] _sortSound(int[] Snd, int nR, int n3, int n5, int n7)
        {
            int baseSound = nR;
            if(baseSound > n3)
            {
                baseSound = n3;
                Snd[0] = n3; Snd[1] = n5; Snd[2] = n7; Snd[3] = nR;
            }
            if (baseSound > n5)
            {
                baseSound = n5;
                Snd[0] = n5; Snd[1] = n7; Snd[2] = nR; Snd[3] = n3;

            }
            if (baseSound > n7)
            {
                baseSound = n7;
                Snd[0] = n7; Snd[1] = nR; Snd[2] = n3; Snd[3] = n5;
            }
            return Snd;
        }*/

        public SoundPlayer setSound(int note) //음을 지정합니다
        {
            SoundPlayer Sound = null;
            if (note == _C) Sound = new SoundPlayer(@"C:\Piano\0.wav");
            if (note == 1) Sound = new SoundPlayer(@"C:\Piano\1.wav");
            if (note == _D) Sound = new SoundPlayer(@"C:\Piano\2.wav");
            if (note == 3) Sound = new SoundPlayer(@"C:\Piano\3.wav");
            if (note == _E) Sound = new SoundPlayer(@"C:\Piano\4.wav");
            if (note == _F) Sound = new SoundPlayer(@"C:\Piano\5.wav");
            if (note == 6) Sound = new SoundPlayer(@"C:\Piano\6.wav");
            if (note == _G) Sound = new SoundPlayer(@"C:\Piano\7.wav");
            if (note == 8) Sound = new SoundPlayer(@"C:\Piano\8.wav");
            if (note == _A) Sound = new SoundPlayer(@"C:\Piano\9.wav");
            if (note == 10) Sound = new SoundPlayer(@"C:\Piano\10.wav");
            if (note == _B) Sound = new SoundPlayer(@"C:\Piano\11.wav");
            return Sound;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nowPlaying > 15) stopB_Click(sender, e);

            noteR = getCode(noteR);
            int[] sound = { noteR, note3, note5, note7 };
            Array.Sort(sound); //낮은 음부터 재생하도록 음을 정렬합니다.
            noteSound = setSound(sound[_4bit]);
            noteSound.Play();
            
            _4bit++;
            if (_4bit % 4 == 0)
            {
                _4bit = 0;
                nowPlaying++;
            }
        }

        private void stopB_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1 = new Timer();
            timer1.Enabled = false;
            timer1.Interval = 1000 *60 / Int32.Parse(BPM_Box.Text);
            timer1.Tick += new EventHandler(timer1_Tick);
            nowPlaying = 0;
            _4bit = 0;
            
        }

        private void playB_Click(object sender, EventArgs e)
        {
            String getBPM = BPM_Box.Text;
            timer1.Start();
            timer1.Interval = 1000 *60 / Int32.Parse(BPM_Box.Text);

            
        }

        private void BPM_Box_KeyPress(object sender, KeyPressEventArgs e) //BPM 텍스트를 숫자만 받도록 합니다.
        {
            if(!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리합니다.
                {
                    e.Handled = true;
                }
        }
    }
}
