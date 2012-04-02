using Gtk;

class Vidmot : Window
{
    // hbTNS is a hbox Three Note State view.
    SNote snote1,
          snote2,
          snote3,
          snote4;
    //VBox todo,
    //     doing,
    //     done,
    VBox     total;
    //HBox hbTNS,
    HBox     hbButtons;
    //Label lblTodo,
    //      lblDoing,
    //      lblDone;
    Button btnNew,
           btnQuit;
    // Test
    TNS tns;
    // EndTest

    public Vidmot() : base("Dagateljari")
    {
        snote1 = new SNote();
        snote2 = new SNote();
        snote3 = new SNote();
        snote4 = new SNote();
        //todo = new VBox();
        //doing = new VBox();
        //done = new VBox();
        total = new VBox();
        //hbTNS = new HBox();
        hbButtons = new HBox();
        //lblTodo = new Label("To do:");
        //lblDoing = new Label("Doing:");
        //lblDone = new Label("Done:");
        btnNew = new Button("New note");
        btnQuit = new Button("Close");

        DeleteEvent += delegate
        {
            Application.Quit();
        };

        btnQuit.Clicked += delegate
        {
            Application.Quit();
        };

        //todo.PackStart(lblTodo, false, false, 10);
        //doing.PackStart(lblDoing, false, false, 10);
        //done.PackStart(lblDone, false, false, 10);

        //todo.PackStart(snote1, false, false, 10);
        //doing.PackStart(snote2, false, false, 10);
        //done.PackStart(snote3, false, false, 10);
        //done.PackStart(snote4, false, false, 10);
        
        // Test
        tns = new TNS();
        total.PackStart(tns, true, true, 0);
        tns.addSNote(snote1, 0);
        tns.addSNote(snote2, 1);
        tns.addSNote(snote3, 2);
        tns.addSNote(snote4, 2);
        // EndTest

        //hbTNS.Add(todo);
        //hbTNS.Add(doing);
        //hbTNS.Add(done);

        hbButtons.Add(btnNew);
        hbButtons.Add(btnQuit);

        //total.PackStart(hbTNS, true, true, 0);
        total.PackStart(hbButtons, false, false, 10);

        snote1.updateInfo("title1", "comment", "date", "prio");
        snote2.updateInfo("title2", "comment", "date", "prio");
        snote3.updateInfo("title3", "comment", "date", "prio");
        snote4.updateInfo("title4", "comment", "date", "prio");

        Add(total);

        ShowAll();
    }

/*    class SNote : VBox
    {
        Label title,
              comment,
              date,
              prio;

        public SNote() : base()
        {
            title = new Label();
            comment = new Label();
            date = new Label();
            prio = new Label();

            Add(title);
            Add(comment);
            Add(date);
            Add(prio);

            ShowAll();
        }

        public void updateInfo(string title, string comment, string date, string prio)
        {
            this.title.Text = title;
            this.comment.Text = comment;
            this.date.Text = date;
            this.prio.Text = prio;
        }
    }*/

    public static void Main()
    {
        Application.Init();
        new Vidmot();
        Application.Run();
    }
}
