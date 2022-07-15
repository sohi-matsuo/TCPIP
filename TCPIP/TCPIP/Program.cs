using System;
class Program
{
    public static void Main()
    {
        SocketServer();
    }
    public static void SocketServer()
    {
        //ListenするIPアドレスを設定
        //ipアドレスの文字列をIPAddressにインスタンスに変換する
        string ipString = "127.0.0.1";
        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ipString);

        //Listenするポート番号を設定
        int port = 2000;

        //TCP ネットワーク クライアントからの接続をListenする
        System.Net.Sockets.TcpListener Server = new System.Net.Sockets.TcpListener(port);

        //Listenを開始する(イメージ：電話をかけている状態)
        Server.Start();

        //Server側のIPアドレスとポート番号を取得するためにLocalEndpointを使用する
        //その際にSystem.Net.IPEndPointにキャストする必要がある
        Console.WriteLine("Listenを開始しました({0}:{1})。",
            ((System.Net.IPEndPoint)Server.LocalEndpoint).Address,
            ((System.Net.IPEndPoint)Server.LocalEndpoint).Port);

        //(AcceptTcpClient)保留中の接続要求があったら受け入れる(イメージ：受話器を上げる状態)
        //複数の接続が可能
        System.Net.Sockets.TcpClient Client = Server.AcceptTcpClient();
        Console.WriteLine("クライアント({0}:{1})と接続しました。",
        ((System.Net.IPEndPoint)Client.Client.RemoteEndPoint).Address,
        ((System.Net.IPEndPoint)Client.Client.RemoteEndPoint).Port);

        //ネットワーク上のデータの送受信にGetSteamを使用
        System.Net.Sockets.NetworkStream ns = Client.GetStream();

        //string型からbyte配列型に変更する(エンコードの指定も必要になるためUTF8に指定)
        //string型からbyte配列型に変更する時にGetBytesを使用する
        //文字列(string型)はUnicodeでは送信することができないのでbyte型として変更する
        System.Text.Encoding nc = System.Text.Encoding.UTF8;

        bool inputrun = true;
        bool run = true;

        //データの長さが分からない時に少しづつデータを溜め込む”MemoryStream”を使う
        //メモリ領域は必要に応じて拡張される
        System.IO.MemoryStream ms = new System.IO.MemoryStream();

        int c;
        string str = "end";
        
        byte[] resbytes = new byte[256];

        int Size = 0;
        while (run)
        {
            //送信されたデータを読み取り、バイト配列に格納する
            while (inputrun)
            {
                //byte[] resbytes = new byte[256];
                //int Size = 0;
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
            string resmsg = nc.GetString(resbytes, 0, Size);
            //string resmsg = nc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            //resmsg = string.Empty;
            
            //end入力がされた時の判定
            string aftertrim = resmsg.Trim();
            c = string.Compare(aftertrim, str, true);
            if (c == 0)
            {
                Console.WriteLine("{0}と入力されました。\t終了します。", resmsg);
                Console.WriteLine("リターンキーを入力してください。Listenerを閉じます。");
               run = false;
            }
            else
            {
                Console.WriteLine("受信した内容を表示します。");
                Console.WriteLine(resmsg);
                inputrun = true;

            }
        }
        ms.Close();

        Server.Stop();
        Console.WriteLine("Listenerを閉じました。");
        
        //リターンキー入力後、意図的に終了させる
        
        Console.ReadLine();
        Client.Close();

    }
}



