using Gtk;
using System;

class Vidmot : Window
{
    // TNS (Three Note State) is a view.
    // SNA (Sticky Note Adder) is a view.
    // snote is an array of SNotes.
    // snCount is the next empty slot
    // in snote.
    SNote[] snote;
    int snCount;
    VBox total;
    HBox hbButtons;
    TNS tns;
    SNA sna;
    Button btnTnsL,
           btnSnaL,
           btnTnsR,
           btnSnaR;

    public Vidmot() : base("Dagateljari")
    {
        total = new VBox();
        tns = new TNS();
        sna = new SNA();
        hbButtons = new HBox();
        btnTnsL = new Button("New note");
        btnSnaL = new Button("Save & Close");
        btnTnsR = new Button("Close");
        btnSnaR = new Button("Cancel & Return");
        // Bound to 100 SNotes.
        snote = new SNote[100];
        snCount = 0;

        hbButtons.Add(btnTnsL);
        hbButtons.Add(btnTnsR);

        btnTnsR.Clicked += delegate
        {
            Application.Quit();
        };

        btnSnaR.Clicked += onBtnSnaRClicked;

        btnTnsL.Clicked += onBtnTnsClicked;

        btnSnaL.Clicked += onBtnSnaLClicked;

        DeleteEvent += delegate
        {
            Application.Quit();
        };

        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);

        Add(total);

        ShowAll();
    }

    private void onBtnTnsClicked(
            object source,
            EventArgs args)
    {
        total.Remove(tns);
        total.Remove(hbButtons);
        total.PackStart(sna, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);
        hbButtons.Remove(btnTnsL);
        hbButtons.Remove(btnTnsR);
        hbButtons.Add(btnSnaL);
        hbButtons.Add(btnSnaR);
        ShowAll();
    }

    private void onBtnSnaLClicked(
            object source,
            EventArgs args)
    {
        // Demand title for each SNote.
        if(sna.getTitle() != "") 
        {
        snote[snCount] = new SNote();
        snote[snCount] = createSNote(); 
        // Stuck at adding to todo.
        tns.addSNote(snote[snCount++], 0);
        }
        total.Remove(sna);
        total.Remove(hbButtons);
        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);
        hbButtons.Remove(btnSnaL);
        hbButtons.Remove(btnSnaR);
        hbButtons.Add(btnTnsL);
        hbButtons.Add(btnTnsR);
        ShowAll();
    }

    private void onBtnSnaRClicked(
            object source,
            EventArgs args)
    {
        total.Remove(sna);
        total.Remove(hbButtons);
        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);
        hbButtons.Remove(btnSnaL);
        hbButtons.Remove(btnSnaR);
        hbButtons.Add(btnTnsL);
        hbButtons.Add(btnTnsR);
        ShowAll();
    }

    private SNote createSNote()
    {
        SNote snote = new SNote();
        snote.updateInfo(
                sna.getTitle(),
                sna.getComment(),
                sna.getDate(),
                sna.getPrio());
        return snote;
    }

    public static void Main()
    {
        Application.Init();
        new Vidmot();
        Application.Run();
    }
}
