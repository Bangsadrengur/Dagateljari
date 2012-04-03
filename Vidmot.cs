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
    Button btnTnsL,
           btnSnaL,
           btnTnsR,
           btnSnaR,
           btnSnaChangeL,
           btnSnaChangeR;
    SNote messagePass;

    // Window constructor.
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
        btnSnaChangeL = new Button("Save & Close");
        btnSnaChangeR = new Button("Cancel & Return");
        // Bound to 100 SNotes.
        snote = new SNote[100];
        snCount = 0;
        messagePass = new SNote(this);

        hbButtons.Add(btnTnsL);
        hbButtons.Add(btnTnsR);

        btnTnsR.Clicked += delegate
        {
            Application.Quit();
        };

        btnSnaR.Clicked += onBtnSnaRClicked;
        btnTnsL.Clicked += onBtnTnsClicked;
        btnSnaL.Clicked += onBtnSnaLClicked;
        btnSnaChangeL.Clicked += onBtnSnaCLClicked;
        btnSnaChangeR.Clicked += onBtnSnaCRClicked;

        DeleteEvent += delegate
        {
            Application.Quit();
        };

        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);

        Add(total);

        // Set initial window size.
        SetDefaultSize(300,400);
        ShowAll();
    }

    // Usage:  Vidmot.changeSNote(snote);
    // Before: View is TNS, snote exists.
    // After:  View is SNA where fields
    //         are set to snote values.
    public void changeSNote(SNote snote)
    {
        total.Remove(tns);
        total.Remove(hbButtons);
        total.PackStart(sna, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);
        hbButtons.Remove(btnTnsL);
        hbButtons.Remove(btnTnsR);
        hbButtons.Add(btnSnaChangeL);
        hbButtons.Add(btnSnaChangeR);
        ShowAll();

        string[] info = snote.getInfo();
        sna.setTitle(info[0]);
        sna.setComment(info[1]);
        sna.setDate(info[2]);
        sna.setPrio(info[3]);
        sna.setStatus(snote.getStatus());
        this.messagePass = snote;
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
        total.Remove(sna);
        total.Remove(hbButtons);
        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);
        hbButtons.Remove(btnSnaChangeL);
        hbButtons.Remove(btnSnaChangeR);
        hbButtons.Add(btnTnsL);
        hbButtons.Add(btnTnsR);
        ShowAll();
    }

    // Before: View is TNS.
    // After:  View is SNA.
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
        sna.setTitle("");
        sna.setComment("");
        sna.setDate("");
        sna.setPrio("");
        ShowAll();
    }

    // Before: View is SNA.
    // After:  View is TNS. A new SNote
    //         exists with values from SNA
    //         fields if the title field was
    //         not empty.
    private void onBtnSnaLClicked(
            object source,
            EventArgs args)
    {
        // Demand title for each SNote.
        if(sna.getTitle() != "") 
        {
        snote[snCount] = new SNote(this);
        snote[snCount] = createSNote(); 
        tns.addSNote(snote[snCount++], sna.getStatus());
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

    // Before: View is SNA in change mode.
    // After:  View is TNS.
    private void onBtnSnaCRClicked(
            object source,
            EventArgs args)
    {
        total.Remove(sna);
        total.Remove(hbButtons);
        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);
        hbButtons.Remove(btnSnaChangeL);
        hbButtons.Remove(btnSnaChangeR);
        hbButtons.Add(btnTnsL);
        hbButtons.Add(btnTnsR);
        ShowAll();
    }

    // Before: View is SNA.
    // After:  View is TNS.
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

    // Start, build and run.
    public static void Main()
    {
        Application.Init();
        new Vidmot();
        Application.Run();
    }
}
