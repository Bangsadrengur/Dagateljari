using Gtk;
// Todo: How to track snotes. snotes tracked in parent?
class TNS : HBox
{
    VBox todo,
         doing,
         done;
    Label lblTodo,
          lblDoing,
          lblDone;

    public TNS() : base()
    {
        todo = new VBox();
        doing = new VBox();
        done = new VBox();
        lblTodo = new Label("To do:");
        lblDoing = new Label("Doing:");
        lblDone = new Label("Done:");

        todo.PackStart(lblTodo, false, false, 10);
        doing.PackStart(lblDoing, false, false, 10);
        done.PackStart(lblDone, false, false, 10);

        Add(todo);
        Add(doing);
        Add(done);
    }

    // state is !(1||2) for todo, 1 doing 2 done.
    public void addSNote(SNote snote, int state)
    {
        switch(state)
        {
            case 1:
                doing.PackStart(snote, false, false, 10);
                break;
            case 2:
                done.PackStart(snote, false, false, 10);
                break;
            default:
                todo.PackStart(snote, false, false, 10);
                break;
        }
    }
}
class SNote : VBox
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
}
