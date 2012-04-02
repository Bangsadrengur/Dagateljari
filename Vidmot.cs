using Gtk;
using System;

class Vidmot : Window
{
    // TNS (Three Note State) is a view.
    SNote snote1,
          snote2,
          snote3,
          snote4;
    VBox total;
    HBox hbButtons;
    TNS tns;
    SNA sna;
    Button btnTnsL,
           btnSnaL,
           btnR;

    public Vidmot() : base("Dagateljari")
    {
        snote1 = new SNote();
        snote2 = new SNote();
        snote3 = new SNote();
        snote4 = new SNote();
        total = new VBox();
        tns = new TNS();
        sna = new SNA();
        hbButtons = new HBox();
        btnTnsL = new Button("New note");
        btnSnaL = new Button("Save & Close");
        btnR = new Button("Close");

        hbButtons.Add(btnTnsL);
        hbButtons.Add(btnR);

        btnR.Clicked += delegate
        {
            Application.Quit();
        };

        btnTnsL.Clicked += onBtnTnsClicked;

        btnSnaL.Clicked += onBtnSnaClicked;

        DeleteEvent += delegate
        {
            Application.Quit();
        };

        tns.addSNote(snote1, 0);
        tns.addSNote(snote2, 1);
        tns.addSNote(snote3, 2);
        tns.addSNote(snote4, 2);

        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);

        snote1.updateInfo("title1", "comment", 
                "date", "prio");
        snote2.updateInfo("title2", "comment", 
                "date", "prio");
        snote3.updateInfo("title3", "comment", 
                "date", "prio");
        snote4.updateInfo("title4", "comment", 
                "date", "prio");

        Add(total);

        ShowAll();
    }

    private void onBtnTnsClicked(object source,
            EventArgs args)
    {
        total.Remove(tns);
        total.Remove(hbButtons);
        total.PackStart(sna, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);
        hbButtons.Remove(btnTnsL);
        hbButtons.Remove(btnR);
        hbButtons.Add(btnSnaL);
        hbButtons.Add(btnR);
        ShowAll();
    }

    private void onBtnSnaClicked(object source,
            EventArgs args)
    {
        total.Remove(sna);
        total.Remove(hbButtons);
        total.PackStart(tns, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);
        hbButtons.Remove(btnSnaL);
        hbButtons.Remove(btnR);
        hbButtons.Add(btnTnsL);
        hbButtons.Add(btnR);
        ShowAll();
    }

    public static void Main()
    {
        Application.Init();
        new Vidmot();
        Application.Run();
    }
}
