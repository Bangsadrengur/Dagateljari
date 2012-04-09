using System;
using Transport;

class Loader
{
    static SNote[] snote;
    static int snCount;
    static Value table;
    static string filename = "calendar.dat";

    public static void writeSNotes(SNote[] snote, int snCount)
    {
        table = Value.MakeTable();
        int i=0;
        int n=0;
        while(n!=snCount)
        {
            string[] info = snote[n].getInfo();
            int status = snote[n].getStatus();
            table[i.ToString()] = info[0];
            i++;
            table[i.ToString()] = info[1];
            i++;
            table[i.ToString()] = info[2];
            i++;
            table[i.ToString()] = info[3];
            i++;
            table[i.ToString()] = status;
            i++;
            n++;
        }
        Value.SaveFile(table, filename);
    }

    public static void readSNotes(Vidmot parent)
    {
        snote = new SNote[100];
        try
        {
            table = Value.LoadFile(filename);
        }
        catch(Exception)
        {
            table = null;
        }
        if(table==null) 
        {
            snCount=0; 
            return;
        }
        int i=0,
            n=0;
        while(table[i.ToString()] != null)
        {
            string[] info = new string[4];
            info[0] = (string) table[i.ToString()];
            i++;
            info[1] = (string) table[i.ToString()];
            i++;
            info[2] = (string) table[i.ToString()];
            i++;
            info[3] = (string) table[i.ToString()];
            i++;
            int status = (int) table[i.ToString()];
            i++;
            snote[n] = new SNote(parent);
            snote[n].updateInfo(
                    info[0],
                    info[1],
                    info[2],
                    info[3],
                    status);
            n++;
        }
        snCount = n;
    }

    public static int getSnCount()
    {
        return snCount;
    }

    public static SNote[] getSNotes()
    {
        return snote;
    }
}

