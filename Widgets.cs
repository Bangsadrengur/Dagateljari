using Gtk;
using System;
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

// Change to buttons?
class SNote : Button
{
    VBox vbox;
    Label title,
          comment,
          date,
          prio;
    Vidmot parent;

    public SNote(Vidmot parent) : base()
    {
        vbox = new VBox();
        title = new Label();
        comment = new Label();
        date = new Label();
        prio = new Label();
        this.parent = parent;

        vbox.Add(title);
        vbox.Add(comment);
        vbox.Add(date);
        vbox.Add(prio);

        Clicked += delegate
        {
            Console.WriteLine(this.title.Text);
            parent.changeSNote(this);
        };

        Add(vbox);
    }


    public void updateInfo(string title, string comment, string date, string prio)
    {
        this.title.Text = title;
        this.comment.Text = comment;
        this.date.Text = date;
        this.prio.Text = prio;
    }

    public string[] getInfo()
    {
        string[] info = new string[4];
        info[0] = title.Text;
        info[1] = comment.Text;
        info[2] = date.Text;
        info[3] = prio.Text;
        return info;
    }
}

// SNote Adder.
class SNA : HBox
{
    VBox vboxL,
         vboxR;
    Label lblTitle,
          lblComment,
          lblDate,
          lblPrio;
    Entry entTitle,
          entComment,
          entDate,
          entPrio;

    public SNA() : base()
    {
        vboxL = new VBox();
        vboxR = new VBox();
        lblTitle = new Label("Title");
        lblComment = new Label("Comment");
        lblDate = new Label("Date");
        lblPrio = new Label("Prio");
        entTitle = new Entry();
        entComment = new Entry();
        entDate = new Entry();
        entPrio = new Entry();

        vboxL.Add(lblTitle);
        vboxL.Add(lblComment);
        vboxL.Add(lblDate);
        vboxL.Add(lblPrio);

        vboxR.Add(entTitle);
        vboxR.Add(entComment);
        vboxR.Add(entDate);
        vboxR.Add(entPrio);

        Add(vboxL);
        Add(vboxR);
    }

    public string getTitle()
    {
        return entTitle.Text;
    }

    public string getComment()
    {
        return entComment.Text;
    }

    public string getDate()
    {
        return entDate.Text;
    }

    public string getPrio()
    {
        return entPrio.Text;
    }

    public void setTitle(string title)
    {
        entTitle.Text = title;
    }

    public void setComment(string comment)
    {
        entComment.Text = comment;
    }

    public void setDate(string date)
    {
        entDate.Text = date;
    }

    public void setPrio(string prio)
    {
        entPrio.Text = prio;
    }
}
