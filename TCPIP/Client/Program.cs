using System;
using System.IO;
using System.Text;

class Program
{

    public static StringBuilder AppendString(StringBuilder outputstring, out int status, ConsoleKeyInfo keyinfo, out Char ch)
    {
        ch = Char.MinValue;
        status = -1;
        switch (keyinfo.Key)
        {
            case ConsoleKey.Escape:
                // ESCキーが押された場合は、入力がキャンセルされたものとしてnullを返す
                status = 0;
                return null;

            case ConsoleKey.Enter:
                // Enterキーが押された場合は、入力が確定されたものとしてバッファに格納した文字列を返す
                status = 0;
                return outputstring;

            case ConsoleKey.Backspace:
                // BackSpaceキーが押された場合は、バッファから最後の一文字を削除する
                if (0 < outputstring.Length)
                {
                    outputstring.Length -= 1;
                    ch = (Char)8;
                }
                else
                {
                    // 削除できる文字がない場合は、ビープ音を鳴らす
                    Console.Beep();
                }
                break;
            default:
                ch = keyinfo.KeyChar;
                if (Char.IsLetter(keyinfo.KeyChar))
                {
                    // 入力された文字がアルファベットなどの文字の場合
                    if ((keyinfo.Modifiers & ConsoleModifiers.Shift) == 0)
                    {
                        // Shiftキーが押されていない場合は、入力された文字をそのままバッファに追加する
                        outputstring.Append(ch);
                    }
                    else
                    {
                        // Shiftキーが押されている場合は、CapsLockキーの状態に応じて大文字または小文字にする
                        if (Console.CapsLock)
                        {
                            ch = Char.ToLower(ch);
                            outputstring.Append(ch);
                        }
                        else
                        {
                            ch = Char.ToUpper(ch);
                            outputstring.Append(ch);
                        }
                    }
                }
                else if (!Char.IsControl(keyinfo.KeyChar))
                {
                    // 入力された文字が制御文字以外(数字や記号)の場合は、そのままバッファに追加する
                    outputstring.Append(ch);
                }
                else
                {
                    ch = Char.MinValue;
                    // コントロールキーやファンクションキーが入力された場合は、ビープ音を鳴らす
                    Console.Beep();
                }
                break;
        }
        return outputstring;
    }
    public static void Main(string[] args)
    {

        string ipOrHost;

        if (args.Length == 0)
        {
            ipOrHost = "127.0.0.1";
        }
        else
        {
            ipOrHost = args[0];
        }

        //サーバーのIPアドレスとポート番号を設定

        int port = 2000;

        //TCPネットワークサービス用のクライアント接続を提供する
        System.Net.Sockets.TcpClient TCP = new System.Net.Sockets.TcpClient(ipOrHost, port);
        Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
                    ((System.Net.IPEndPoint)TCP.Client.RemoteEndPoint).Address,
                    ((System.Net.IPEndPoint)TCP.Client.RemoteEndPoint).Port,
                    ((System.Net.IPEndPoint)TCP.Client.LocalEndPoint).Address,
                    ((System.Net.IPEndPoint)TCP.Client.LocalEndPoint).Port);

        //ネットワーク上のデータの送受信にGetSteamを使用
        System.Net.Sockets.NetworkStream ns = TCP.GetStream();

        //サーバーに送るデータを入力
        //何も入力されていない時・リターンキーのみ・end入力された時はループを抜ける

        bool inputrun = true;
        bool run = true;

        int c;
        int Size = 0;

        string str = "end";

        byte[] resbytes = new byte[256];

        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        var sendstring = new StringBuilder();
        int status;


        while (run)
        {
            ////Client側からメッセージ送信
            ////コンソールから文字が入力されたかを判定する(入力された時はtrue)
            //if (Console.KeyAvailable == true)
            //{
            //    //Console.WriteLine("[文字を入力し、Enterキーで送信してください]");

            //    //ReadLine()は同期する(入力待ちの状態になり、以降の処理が止まる)
            //    string sendMsg = Console.ReadLine();

            //    c = string.Compare(sendMsg.Trim(), str, true);
            //    if (sendMsg == null || c == 0)
            //    {
            //        run = false;
            //    }
            //    //string型からbyte配列型に変更する(エンコードの指定も必要になるためUTF8に指定)
            //    //string型からbyte配列型に変更する時にGetBytesを使用する
            //    //文字列(string型)はUnicodeでは送信することができないのでbyte型として変更する
            //    System.Text.Encoding NC = System.Text.Encoding.UTF8;
            //    byte[] sendbytes = NC.GetBytes(sendMsg + '\n');

            //    //データを送信
            //    ns.Write(sendbytes, 0, sendbytes.Length);
            //    //Console.WriteLine(sendMsg);
            //}

            //入力された文字を一文字ずつ取得していく
            if (Console.KeyAvailable)
            {
                //Readkey(true)→入力文字をコンソール上に非表示に設定
                ConsoleKeyInfo keyinfo = Console.ReadKey(true);
                Char ch;

                sendstring = AppendString(sendstring, out status, keyinfo, out ch);
                //charの最小有効値を表す
                if (ch != Char.MinValue)
                {
                    Console.Write(ch);
                    //バックスペースなら
                    if ((int)ch == 8)
                    {
                        //苦しい・・・
                        Console.Write(' ');
                        Console.Write(ch);
                    }
                }
                string w = sendstring.ToString();
                if (sendstring == null)
                {
                    break;
                }
                if (status == 0)
                {
                    sendstring.Clear();
                    Console.WriteLine("");
                    //Console.WriteLine(w);

                    //文字列(string型)はUnicodeでは送信することができないのでbyte型として変更する
                    System.Text.Encoding NC = System.Text.Encoding.UTF8;
                    byte[] sendbytes = NC.GetBytes(w + '\n');

                    //データを送信
                    ns.Write(sendbytes, 0, sendbytes.Length);

                    //end入力がされた時
                    c = string.Compare(w.Trim(), str, true);
                    if (c == 0)
                    {
                        run = false;

                    }
                    //if (w.Trim().ToUpper() == "END")
                    //{
                    //    break;
                    //}
                }
            }
            //Client側のデータ受信
            if (ns.DataAvailable == true)
            {
                while (inputrun)
                {
                    //送信されたデータを読み取り、バイト配列に格納する
                    //NetworkStream.Read(格納するByte配列,データを格納を開始する場所，読み取るバイト数)
                    Size = ns.Read(resbytes, 0, resbytes.Length);

                    //読み取るデータが無くなった時・改行のみ入力された時・改行が来た時
                    //データを読み取る処理を終了する

                    if (Size == 0 || resbytes[Size - 1] == '\n')
                    {
                        inputrun = false;
                    }

                    //読み取ったデータをresbytes(byte型)に格納する
                    ms.Write(resbytes, 0, Size);
                }
                //読み取ったbyte型をstring型に変換する
                System.Text.Encoding nc = System.Text.Encoding.UTF8;
                string resmsg = nc.GetString(resbytes, 0, Size);

                //end入力がされた時の判定
                string aftertrim = resmsg.Trim();
                c = string.Compare(aftertrim, str, true);
                if (c == 0)
                {
                    Console.WriteLine("[{0}]を受信しました。", aftertrim);
                    run = false;
                }
                else
                {
                    Console.WriteLine("[受信した内容を表示します]");
                    Console.WriteLine(resmsg);
                    inputrun = true;
                }
            }
        }

        Console.WriteLine("Clientを閉じます。リターンキーを入力してください。");
        Console.ReadLine();
        Console.WriteLine("Clientを閉じました。");
        TCP.Close();
    }
}




