using System;
using Transport;

class Loader
{
    // Data invariant:
    // * snote is an array of SNotes.
    // * snCount is a counter for SNotes in
    //   snote.
    // * table is a Transport.Value table.
    // * filename points to file for storing SNotes.
    static SNote[] snote;
    static int snCount;
    static Value table;
    static string filename = "calendar.dat";

    // Before: snote is an array of SNotes and snCount points
    // is the upper bound of the number of SNotes in the array.
    // After:  SNotes of snote have been stored in filename.
    public static void writeSNotes(SNote[] snote, int snCount)
    {
        // Data invariant: 
        // * i is an index for table values.
        // * n is an index for snote values.
        table = Value.MakeTable();
        int i=0;
        int n=0;
        // Iterate over all possible places with
        // SNotes in snote.
        while(n!=snCount)
        {
            // If SNote in snote has title, 
            // write that SNote to file,
            // if not then move on to next
            // possible SNote in snote.
            if(snote[n].getInfo()[0] != "")
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
            else
            {
                n++;
            }
        }
        Value.SaveFile(table, filename);
    }

    // Before: parent is a class with a SNote array.
    // After:  SNotes with values from filename
    //         have been set up and the SNotes are
    //         loaded to snote. SNotes have a pointer
    //         to parent.
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
        // If nothing loaded from filename 
        // then snote is initialized, empty
        // and correct.
        if(table==null) 
        {
            snCount=0; 
            return;
        }
        // Data invariant:
        // * i is an index to table values.
        // * n is an index to snote values.
        int i=0,
            n=0;
        // After: Values have been loaded from
        // table (from filename) into n SNotes
        // and the SNotes stored in snote.
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

    // get snCount.
    public static int getSnCount()
    {
        return snCount;
    }

    // get snote.
    public static SNote[] getSNotes()
    {
        return snote;
    }
}

