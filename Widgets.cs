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

    public void removeSNote(SNote snote)
    {
        switch(snote.getStatus())
        {
            case 1:
                doing.Remove(snote);
                break;
            case 2:
                done.Remove(snote);
                break;
            default:
                todo.Remove(snote);
                break;
        }
    }
}

class SNote : Button
{
    VBox vbox;
    Label title,
          comment,
          date,
          prio;
    int status;

    public SNote(Vidmot parent) : base()
    {
        vbox = new VBox();
        title = new Label();
        comment = new Label();
        date = new Label();
        prio = new Label();
        status = 0;

        vbox.Add(title);
        vbox.Add(comment);
        vbox.Add(date);
        vbox.Add(prio);

        Clicked += delegate
        {
            parent.changeSNote(this);
        };

        Add(vbox);
    }

    public void updateInfo(
            string title, 
            string comment, 
            string date, 
            string prio,
            int status)
    {
        this.title.Text = title;
        this.comment.Text = comment;
        this.date.Text = date;
        this.prio.Text = prio;
        this.status = status;
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

    public void setStatus(int status)
    {
        this.status = status;
    }

    public int getStatus()
    {
        return status;
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
          lblPrio,
          lblTodo,
          lblDoing,
          lblDone;
    Entry entTitle,
          entComment,
          entDate,
          entPrio;
    RadioButton
        rbTodo,
        rbDoing,
        rbDone;

    public SNA() : base()
    {
        vboxL = new VBox();
        vboxR = new VBox();
        lblTitle = new Label("Title");
        lblComment = new Label("Comment");
        lblDate = new Label("Date");
        lblPrio = new Label("Prio");
        lblTodo = new Label("Status:");
        lblDoing = new Label("");
        lblDone = new Label("");
        entTitle = new Entry();
        entComment = new Entry();
        entDate = new Entry();
        entPrio = new Entry();
        rbTodo = new RadioButton("Not started.");
        rbDoing = new RadioButton("In progress.");
        rbDone = new RadioButton("Work completed.");

        rbDoing.Group = rbTodo.Group;
        rbDone.Group = rbTodo.Group;

        vboxL.Add(lblTitle);
        vboxL.Add(lblComment);
        vboxL.Add(lblDate);
        vboxL.Add(lblPrio);
        vboxL.Add(lblTodo);
        vboxL.Add(lblDoing);
        vboxL.Add(lblDone);

        vboxR.Add(entTitle);
        vboxR.Add(entComment);
        vboxR.Add(entDate);
        vboxR.Add(entPrio);
        vboxR.Add(rbTodo);
        vboxR.Add(rbDoing);
        vboxR.Add(rbDone);

        Add(vboxL);
        Add(vboxR);
    }

    public int getStatus()
    {
        if(rbDoing.Active) return 1;
        if(rbDone.Active) return 2;
        return 0;
    }

    public void setStatus(int status)
    {
        switch(status)
        {
            case 1:
                rbDoing.Active = true;
                break;
            case 2:
                rbDone.Active = true;
                break;
            default:
                rbTodo.Active = true;
                break;
        }
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
