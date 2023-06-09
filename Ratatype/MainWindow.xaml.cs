using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ratatype
{

    public partial class MainWindow : Window
    {

        Random random = new Random();
        DispatcherTimer timer;
        int Fails = 0;

        public MainWindow()
        {
            InitializeComponent();
            StopBtn.IsEnabled = false;
            LineWriter.IsReadOnly = true;
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
        }


        /// =========================================   Кнопки клавиатуры
        private void ShiftUnpreesSymbols()
        {
            _Apostrophe.Text = "`"; _1.Text = "1"; _2.Text = "2"; _3.Text = "3";
            _4.Text = "4"; _5.Text = "5"; _6.Text = "6"; _7.Text = "7"; _8.Text = "8";
            _9.Text = "9"; _0.Text = "0"; _Dash.Text = "-"; _Equals.Text = "=";
            _SquareBracketRight.Text = "]"; _SquareBracketLeft.Text = "[";
            _Comma.Text = ","; _Dot.Text = "."; _SlashR.Text = "/"; _SlashL.Text = "\\";
            _Semicolon.Text = ";"; _SingleBrace.Text = "'";
        }
        private void ShiftPressSymbols()
        {
            _Apostrophe.Text = "~"; _1.Text = "!"; _2.Text = "@"; _3.Text = "#";
            _4.Text = "$"; _5.Text = "%"; _6.Text = "^"; _7.Text = "&"; _8.Text = "*";
            _9.Text = "("; _0.Text = ")"; _Dash.Text = "_"; _Equals.Text = "+";
            _SquareBracketRight.Text = "}"; _SquareBracketLeft.Text = "{";
            _Comma.Text = "<"; _Dot.Text = ">"; _SlashR.Text = "?"; _SlashL.Text = "|";
            _Semicolon.Text = ":"; _SingleBrace.Text = "\""; 
        }
        private void LowerLetters()
        {
            _Q.Text = "q"; _W.Text = "w"; _E.Text = "e"; _R.Text = "r"; _T.Text = "t";
            _Y.Text = "y"; _T.Text = "t"; _I.Text = "i"; _O.Text = "o"; _P.Text = "p";
            _A.Text = "a"; _S.Text = "s"; _D.Text = "d"; _F.Text = "f"; _G.Text = "g";
            _H.Text = "h"; _J.Text = "j"; _K.Text = "k"; _L.Text = "l"; _Z.Text = "z"; 
            _X.Text = "x"; _C.Text = "c"; _V.Text = "v"; _B.Text = "b"; _N.Text = "n"; 
            _M.Text = "m"; _U.Text = "u";
        }
        private void CapitalLetter()
        {
            _Q.Text = "Q"; _W.Text = "W"; _E.Text = "E"; _R.Text = "R"; _T.Text = "T";
            _Y.Text = "Y"; _U.Text = "U"; _I.Text = "I"; _O.Text = "O"; _P.Text = "P";
            _A.Text = "A"; _S.Text = "S"; _D.Text = "D"; _F.Text = "F"; _G.Text = "G";
            _H.Text = "H"; _J.Text = "J"; _K.Text = "K"; _L.Text = "L"; _Z.Text = "Z";
            _X.Text = "X"; _C.Text = "C"; _V.Text = "V"; _B.Text = "B"; _N.Text = "N";
            _M.Text = "M"; _U.Text = "U";
        }

        /// =========================================


        /// ========================================= Когда кнопка нажимается 

        bool flagCapsLock = false;
        string StringCheck;
        string CustomString;

        private void ClickKey(object sender, KeyEventArgs e)
        {
            foreach (UIElement temp in (this.Content as Grid).Children)
            {
                if (temp is Grid)
                {
                    foreach (var item in (temp as Grid).Children)
                    {
                        if (item is StackPanel)
                        {
                            foreach (var res in ((StackPanel)item).Children)
                            {
                                if (res is Border)
                                {
                                    if((res as Border).Name == e.Key.ToString())
                                    {
                                        (res as Border).Opacity = 0.5;
                                        if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
                                        {
                                            ShiftPressSymbols();
                                            if (flagCapsLock)
                                            {
                                                LowerLetters();
                                            }
                                            else
                                            {
                                                CapitalLetter();
                                            }
                                        }
                                        if (e.Key == Key.CapsLock)
                                        {
                                            if (!flagCapsLock)
                                            {
                                                flagCapsLock = true;
                                                CapitalLetter();
                                            }
                                            else
                                            {
                                                flagCapsLock = false;
                                                LowerLetters();
                                            }
                                        }
                                        if(e.Key.ToString() == "Back")
                                        {
                                            LineWriter.Foreground = new SolidColorBrush(Colors.Black);
                                        }

                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        /// ========================================= Когда кнопка отжимается
        private void ReturnedAll(object sender, KeyEventArgs e)
        {
            foreach (UIElement temp in (this.Content as Grid).Children)
            {
                if (temp is Grid)
                {
                    foreach (var item in (temp as Grid).Children)
                    {
                        if (item is StackPanel)
                        {
                            foreach (var res in ((StackPanel)item).Children)
                            {
                                if (res is Border)
                                {
                                    if ((res as Border).Name == e.Key.ToString())
                                    {
                                        (res as Border).Opacity = 1;
                                        if (e.Key.ToString() == "LeftShift" || e.Key.ToString() == "RightShift")
                                        {
                                            ShiftUnpreesSymbols();
                                            if (!flagCapsLock)
                                            {
                                                LowerLetters();
                                            }
                                            else
                                            {
                                                CapitalLetter();
                                            }

                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }

            StringCheck = HelperBox.Text;
            CustomString = LineWriter.Text;
            if (CustomString != string.Empty)
            {
                if (CaseSensetive.IsChecked == true)
                {
                    StringCheck = StringCheck.ToLower();
                    CustomString = CustomString.ToLower();
                }
                if (StringCheck[CustomString.Length - 1] != CustomString[CustomString.Length - 1])
                {
                    Fails++;
                    FailsLabel.Content = Fails.ToString();
                    LineWriter.Foreground = new SolidColorBrush(Colors.Red);
                }
                if (StringCheck.Length == CustomString.Length)
                {
                    MessageBox.Show($"Mission completed! \nPrint speed: {SpeedLabel.Content} chars/min \nCount fails: {FailsLabel.Content} \nTo end the task, press Stop");
                }
            }


        }
        /// =====================================================================================





        /// ====================================  Старт тренажера

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            LineWriter.IsReadOnly = false;
            LineWriter.Focus();
            StartBtn.IsEnabled = false;
            StopBtn.IsEnabled = true;
            CaseSensetive.IsEnabled = false;
            SliderDifficulty.IsEnabled = false;
            HelperBox.Text = ChoseDifficult();
            timer.Start();

        }

        /// ====================================  Остановка тренажера
        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {

            LineWriter.IsReadOnly = true;
            StopBtn.IsEnabled = false; 
            StartBtn.IsEnabled = true;
            CaseSensetive.IsEnabled = true;
            SliderDifficulty.IsEnabled = true;
            HelperBox.Text = string.Empty;
            LineWriter.Text = string.Empty;
            CountSecond = 0;
            SpeedLabel.Content = "0";
            FailsLabel.Content = "0";
            Fails = 0;
            timer.Stop();
        }

        // ========================== Вспомогательные методы 
        
        
        // Таймер 
        int CountSecond = 0;
        private void Timer_Tick(object? sender, EventArgs e)
        {
            CountSecond++;
            Speed();
        }

        // Считает скорость печати
        private void Speed()
        {
            SpeedLabel.Content = Math.Round(((double)LineWriter.Text.Length / CountSecond) * 60).ToString();
        }

        // Считывает строку из файла
        private string ChoseDifficult()
        {
            int randomNumber = random.Next(1,4);
            string FileName = string.Format( $"String//difficult{DifficultLabel.Content}_{randomNumber}.txt");
            StreamReader reader = new StreamReader(FileName);
            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }

        // Выбирает сложность печати
        private void SliderDifficulty_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int temp = (int)SliderDifficulty.Value;
            DifficultLabel.Content = temp.ToString();
        }


    }
}
