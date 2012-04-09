using Gtk;
using System;

class Vidmot : Window
{
    // Data invariant:
    // * TNS (Three Note State) is a view.
    // * SNA (Sticky Note Adder) is a view.
    //   SNA has a change mode where SNotes
    //   aren't added but modified.
    // * snote is an array of SNotes.
    // * snCount is the next empty slot
    //   in snote.
    // * messagePass is for holding a referance
    //   to snote that is being changed.
    // * All buttons end with L for left position
    //   or R for right position. Buttons are
    //   packed to the bottom of the window,
    //   two at a time.
    // * btnTns* is a button for TNS view.
    // * btnSna* is a button for SNA view.
    // * btnSnaChange* is a button for 
    //   SNA view in change mode.
    SNote[] snote;
    int snCount;
    VBox total;
    HBox hbButtons;
    TNS tns;
    SNA sna;
    Button btnL,
           btnR;
    SNote messagePass;

    // Window constructor.
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

        Loader.readSNotes(this);
        snote = Loader.getSNotes();
        snCount = Loader.getSnCount();
        int n=0;
        while(snote[n] is SNote)
        {
            tns.addSNote(snote[n], snote[n].getStatus());
            n++;
        }

        hbButtons.Add(btnL);
        hbButtons.Add(btnR);

        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;

        DeleteEvent += delegate
        {
            Loader.writeSNotes(snote, snCount);
            Application.Quit();
        };

        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);

        Add(total);

        // Set initial window size.
        SetDefaultSize(300,400);
        ShowAll();
    }

    private void onBtnTnsLClicked(
            object source,
            EventArgs args)
    {
        // Set new function for btnL
        btnL.Clicked -= onBtnTnsLClicked;
        btnR.Clicked -= onBtnTnsRClicked;
        btnL.Clicked += onBtnSnaLClicked;
        btnR.Clicked += onBtnSnaRClicked;
        updateView(sna, tns);
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
        Loader.writeSNotes(snote, snCount);
        Application.Quit();
    }

    private void onBtnSnaLClicked(
            object source,
            EventArgs args)
    {
        // Demand title for each SNote.
        if(sna.getTitle() != "") 
        {
        snote[snCount] = new SNote(this);
        snote[snCount] = createSNote(); 
        // Stuck at adding to todo.
        tns.addSNote(snote[snCount++], sna.getStatus());
        }
        btnL.Clicked -= onBtnSnaLClicked;
        btnR.Clicked -= onBtnSnaRClicked;
        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;
        updateView(tns, sna);
        ShowAll();
    }

    private void onBtnSnaRClicked(
            object source,
            EventArgs args)
    {
        btnL.Clicked -= onBtnSnaLClicked;
        btnR.Clicked -= onBtnSnaRClicked;
        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;
        updateView(tns, sna);
        ShowAll();
    }

    // Before: View is SNA in change mode. 
    //         messagePass is a non null SNote.
    // After:  messagePass is has been updated
    //         and placed in the appropriate
    //         TNS column.
    //         View is TNS.
    private void onBtnSnaCLClicked(
            object source,
            EventArgs args)
    {
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
        btnL.Clicked -= onBtnSnaCLClicked;
        btnR.Clicked -= onBtnSnaCRClicked;
        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;
        updateView(tns, sna);
        ShowAll();
    }

    // Before: View is SNA in change mode.
    // After:  View is TNS.
    private void onBtnSnaCRClicked(
            object source,
            EventArgs args)
    {
        btnL.Clicked -= onBtnSnaCLClicked;
        btnR.Clicked -= onBtnSnaCRClicked;
        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;
        updateView(tns, sna);
        ShowAll();
    }

    // Usage:  snote = Vidmot.createSNote();
    // Before: The view is SNA.
    // After:  snote is a new SNote with values
    //         from SNA fields.
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

    private void updateView(HBox newView, HBox oldView)
    {
        total.Remove(oldView);
        total.Remove(hbButtons);
        total.PackStart(newView, true, true, 0);
        total.PackStart(hbButtons, false, false, 0);
    }

    // Usage:  Vidmot.changeSNote(snote);
    // Before: View is TNS, snote exists.
    // After:  View is SNA where fields
    //         are set to snote values.
    public void changeSNote(SNote snote)
    {
        btnL.Clicked -= onBtnTnsLClicked;
        btnR.Clicked -= onBtnTnsRClicked;
        btnL.Clicked += onBtnSnaCLClicked;
        btnR.Clicked += onBtnSnaCRClicked;
        updateView(sna, tns);
        ShowAll();

        string[] info = snote.getInfo();
        sna.setTitle(info[0]);
        sna.setComment(info[1]);
        sna.setDate(info[2]);
        sna.setPrio(info[3]);
        sna.setStatus(snote.getStatus());
        this.messagePass = snote;
    }

    // Start, build and run.
    public static void Main()
    {
        Application.Init();
        new Vidmot();
        Application.Run();
    }
}
