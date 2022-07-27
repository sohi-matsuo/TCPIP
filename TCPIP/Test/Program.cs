using System;
using System.Threading.Tasks;

namespace KeyInterruptHandling
{
    class Program3
    {
        static void Main(string[] args)
        {
            string outChar = "a";

            //処理ループ
            while (true)
            {
                //これがメインの処理に該当
                //キー入力チェック。Eが入力されたらプログラム終了。
                if (Console.KeyAvailable)
                {
                    
                    ConsoleKeyInfo k = Console.ReadKey();
                    string outchar = k.KeyChar.ToString();
                    //while () 
                    //{
                    //    string result += outchar;
                    //}

                    if (k.KeyChar == (char)ConsoleKey.Enter)
                    {
                        Console.Write("\n");
                        Console.WriteLine(outchar);
                    }
                    //Console.Write(outChar);
                    if (outchar == "E")
                    {
                        return;
                    }
                }
            }
        }
    }
}

//using System;

//class Sample
//{
//    static void Main()
//    {
//        while (true)
//        { // 無限ループ
//          // 標準入力から一文字読み込む
//            int ch = Console.Read();

//            if (ch == -1)
//            {
//                // 読み込める文字列が無くなった
//                break;
//            }
//            else
//            {
//                // 読み込んだ文字にかっこを付けて標準出力に書き込む
//                var c = (char)ch;

//                Console.Write($"{c}");
//            }
//        }
//    }
//}
