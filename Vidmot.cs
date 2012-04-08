using Gtk;
using System;

class Vidmot : Window
{
    // TNS (Three Note State) is a view.
    // SNA (Sticky Note Adder) is a view.
    // snote is an array of SNotes.
    // snCount is the next empty slot
    // in snote.
    // messagePass is for holding a referance
    // to snote that is being changed.
    SNote[] snote;
    int snCount;
    VBox total;
    HBox hbButtons;
    TNS tns;
    SNA sna;
    Button btnL,
           btnR;
    SNote messagePass;

    public Vidmot() : base("Dagateljari")
    {
        total = new VBox();
        tns = new TNS();
        sna = new SNA();
        hbButtons = new HBox();
        btnL = new Button("Left");
        btnR = new Button("Right");
        // Bound to 100 SNotes.
        snote = new SNote[100];
        snCount = 0;
        messagePass = new SNote(this);

        hbButtons.Add(btnL);
        hbButtons.Add(btnR);

        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;

        DeleteEvent += delegate
        {
            Application.Quit();
        };

        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);

        Add(total);

        SetDefaultSize(300,400);
        ShowAll();
    }

    private void onBtnTnsLClicked(
            object source,
            EventArgs args)
    {
        // Set new function for btnL
        total.Remove(tns);
        total.PackEnd(sna, true, true, 0);
        sna.setTitle("");
        sna.setComment("");
        sna.setDate("");
        sna.setPrio("");
        ShowAll();
    }

    private void onBtnTnsRClicked(
            object source,
            EventArgs args)
    {
        Application.Quit();
    }

    private void onBtnSnaLClicked(
            object source,
            EventArgs args)
    {
        // Set btnL to TNS.
        // Demand title for each SNote.
        if(sna.getTitle() != "") 
        {
        snote[snCount] = new SNote(this);
        snote[snCount] = createSNote(); 
        // Stuck at adding to todo.
        tns.addSNote(snote[snCount++], sna.getStatus());
        }
        total.Remove(sna);
        total.PackStart(tns, true, true, 0);
        ShowAll();
    }

    private void onBtnSnaRClicked(
            object source,
            EventArgs args)
    {
        // Set btns to TNS.
        total.Remove(sna);
        total.PackStart(tns, true, true, 0);
        ShowAll();
    }

    // Updated
    private void onBtnSnaCLClicked(
            object source,
            EventArgs args)
    {
        // Set new function for btnL
        if(sna.getStatus() != messagePass.getStatus())
        {
           tns.removeSNote(messagePass); 
        this.messagePass.updateInfo(
                sna.getTitle(),
                sna.getComment(),
                sna.getDate(),
                sna.getPrio(),
                sna.getStatus());
           tns.addSNote(
                   messagePass, 
                   messagePass.getStatus());
        } else 
        {
        this.messagePass.updateInfo(
                sna.getTitle(),
                sna.getComment(),
                sna.getDate(),
                sna.getPrio(),
                sna.getStatus());
        }
        total.Remove(sna);
        total.PackStart(tns, true, true, 0);
        ShowAll();
    }

    private void onBtnSnaCRClicked(
            object source,
            EventArgs args)
    {
        // Change view to TNS
        total.Remove(sna);
        total.PackStart(tns, true, true, 0);
        ShowAll();
    }


    private SNote createSNote()
    {
        SNote snote = new SNote(this);
        snote.updateInfo(
                sna.getTitle(),
                sna.getComment(),
                sna.getDate(),
                sna.getPrio(),
                sna.getStatus());
        return snote;
    }

    public void changeSNote(SNote snote)
    {
        // Set view to SNA
        total.Remove(tns);
        total.PackStart(sna, true, true, 0);
        ShowAll();

        string[] info = snote.getInfo();
        sna.setTitle(info[0]);
        sna.setComment(info[1]);
        sna.setDate(info[2]);
        sna.setPrio(info[3]);
        sna.setStatus(snote.getStatus());
        this.messagePass = snote;
    }

    public static void Main()
    {
        Application.Init();
        new Vidmot();
        Application.Run();
    }
}
