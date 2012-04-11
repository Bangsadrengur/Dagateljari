using Gtk;
using System;

class Vidmot : Window
{    
    // Data invariant:
    // * tns (Three Note State) is a view.
    // * sna (Sticky Note Adder) is a view.
    //   sna has a change mode where SNotes
    //   aren't added but modified.
    // * snote is an array of SNotes.
    // * snCount is the next empty slot
    //   in snote.
    // * messagePass is for holding a referance
    //   to snote that is being changed.
    // * btnL and btnR are buttons at a constant
    //   place at the bottom of the window. 
    // * btnL is usually connected to some 
    //   active functionality (f.ex. new note, 
    //   save & *).
    // * btnR is usually connected to some
    //   passive functionality such as
    //   (f.ex. close application, return without 
    //   change) 
    // * extraR is a right position button, placed
    //   above btnL and btnR. It is made visible 
    //   when changing SNotes and is for deleting
    //   the SNote currently inspected through
    //   SNA view.
    // * extraL is a left positioned label for
    //   aligning extraR neatly above btnR.
    //   extraL should not contain any text.
    // * extraBox contains and aligns
    //   extraL and extraR.
    // * total aligns widgets for window.
    // * hbButtons contains and aligns
    //   btnL and btnR.

    SNote[] snote;
    int snCount;
    VBox total;
    HBox hbButtons;
    TNS tns;
    SNA sna;
    Button btnL,
           btnR;
    Button extraR;
    Label extraL;
    HBox extraBox;
    SNote messagePass;

    // Window constructor.
    public Vidmot() : base("Dagateljari")
    {
        total = new VBox();
        tns = new TNS();
        sna = new SNA();
        hbButtons = new HBox();
        btnL = new Button("New Sticky Note");
        btnR = new Button("Close Application");
        // Bound to 100 SNotes.
        snote = new SNote[100];
        // Initially 0 SNotes stored in each run.
        snCount = 0;
        messagePass = new SNote(this);
        // Delete function widgets.
        extraR = new Button("Delete this note");
        extraL = new Label("");
        extraBox = new HBox();

        // Load stored SNotes from previous run.
        Loader.readSNotes(this);
        snote = Loader.getSNotes();
        snCount = Loader.getSnCount();
        int n=0;
        while(snote[n] is SNote)
        {
            tns.addSNote(snote[n], snote[n].getStatus());
            n++;
        }

        hbButtons.Homogeneous = true;
        hbButtons.Add(btnL);
        hbButtons.Add(btnR);

        extraBox.Homogeneous = true;
        extraBox.Add(extraL);
        extraBox.Add(extraR);

        // Set functions.
        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;
        extraR.Clicked += onBtnExtraRClicked;

        DeleteEvent += delegate
        {
            Loader.writeSNotes(snote, snCount);
            Application.Quit();
        };

        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);

        Add(total);

        // Set initial window size.
        SetDefaultSize(400,500);
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
        btnL.Label = "Save & Return";
        btnR.Label = "Return";
        updateView(sna, tns, false);
        sna.setTitle("");
        sna.setComment("");
        sna.setDate("");
        sna.setPrio("");
        sna.setStatus(0);
        ShowAll();
    }

    // Before: View is TNS. 
    // After:  Application has closed.
    private void onBtnTnsRClicked(
            object source,
            EventArgs args)
    {
        Loader.writeSNotes(snote, snCount);
        Application.Quit();
    }

    // Before: View is TNS.
    // After:  View is SNA.
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
        btnL.Label = "New Sticky Note";
        btnR.Label = "Close Application";
        updateView(tns, sna, false);
        ShowAll();
    }

    // Before: View is SNA.
    // After:  View is TNS.
    private void onBtnSnaRClicked(
            object source,
            EventArgs args)
    {
        btnL.Clicked -= onBtnSnaLClicked;
        btnR.Clicked -= onBtnSnaRClicked;
        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;
        btnL.Label = "New Sticky Note";
        btnR.Label = "Close Application";
        updateView(tns, sna, false);
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
        btnL.Label = "New Sticky Note";
        btnR.Label = "Close Application";
        updateView(tns, sna, true);
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
        btnL.Label = "New Sticky Note";
        btnR.Label = "Close Application";
        updateView(tns, sna, true);
        ShowAll();
    }

    // Before: View is SNA in change mode
    //         for a selected SNote.
    // After:  View is TNS and selected
    //         SNote was deleted.
    private void onBtnExtraRClicked(
            object source,
            EventArgs args)
    {
        tns.removeSNote(messagePass);
        messagePass.updateInfo(
                "",
                "",
                "",
                "",
                0);
        updateView(tns, sna, true);
        btnL.Clicked -= onBtnSnaCLClicked;
        btnR.Clicked -= onBtnSnaCRClicked;
        btnL.Clicked += onBtnTnsLClicked;
        btnR.Clicked += onBtnTnsRClicked;
        btnL.Label = "New Sticky Note";
        btnR.Label = "Close Application";
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

    // Before: View is oldView. If deleteButton is true, extraBox was
    //         visible in last view or should be visible in next view.
    // After:  View is newView. If deleteButton is true, extraBox has
    //         been removed from view or added to view.
    private void updateView(HBox newView, HBox oldView, bool deleteButton)
    {
        total.Remove(oldView);
        // Check if view was SNA in change mode and remove extraBox in
        // next view if so.
        if(deleteButton && newView == tns) total.Remove(extraBox);
        total.Remove(hbButtons);
        total.PackStart(newView, true, true, 0);
        // Check if next view should be SNA in change mode and add extraBox
        // if so.
        if(deleteButton && newView == sna) total.PackStart(extraBox, false, false, 0);
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
        btnL.Label = "Save & Return";
        btnR.Label = "Return";
        updateView(sna, tns, true);
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
