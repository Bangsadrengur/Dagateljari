// A collection of widgets for Vidmot.cs
// Author: Heimir Þór Kjartansson.

using Gtk;
using System;

// TNS is a three note state widget. It's for displaying
// SNotes in three possible states: todo, doing, done. 
// The seperation between states is achieved by using
// three columns, one for each state.
class TNS : HBox
{
    // Data invariant:
    // * VBox items are columns for states.
    // * Labels for the top of each column.
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
    // After: snote has been added to TNS in column
    // according to state.
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

    // After: snote has been removed from column defined
    // by snote.getStatus().
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

// SNote is a sticky note for containing assignements.
// SNote stores 6 details about note, title, comment, date
// and priority in string form and state (todo, doing done) in
// ,coded' integer form.
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

    // After: SNote has had it's details updates.
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

    // Usage: string[] = SNote.getINfo();
    // After: string contains SNote's string-
    //        stored info.
    public string[] getInfo()
    {
        string[] info = new string[4];
        info[0] = title.Text;
        info[1] = comment.Text;
        info[2] = date.Text;
        info[3] = prio.Text;
        return info;
    }

    // Set status of SNote.
    public void setStatus(int status)
    {
        this.status = status;
    }

    // Get status of SNote.
    public int getStatus()
    {
        return status;
    }
}

// SNA is a sticky note adder widget. It's for displaying
// info fields for SNote creation or modifying.
class SNA : HBox
{
    // Labels and entry boxes for SNote title, 
    // comment, date and priority.
    // Radiobuttons for status.
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

    // Get status from radio buttons.
    public int getStatus()
    {
        if(rbDoing.Active) return 1;
        if(rbDone.Active) return 2;
        return 0;
    }

    // Set radio buttons status.
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

    // Get title from entry field.
    public string getTitle()
    {
        return entTitle.Text;
    }

    // Get comment from entry field.
    public string getComment()
    {
        return entComment.Text;
    }

    // Get date from entry field.
    public string getDate()
    {
        return entDate.Text;
    }

    // Get priority from entry field.
    public string getPrio()
    {
        return entPrio.Text;
    }

    // Set title field.
    public void setTitle(string title)
    {
        entTitle.Text = title;
    }

    // Set comment field.
    public void setComment(string comment)
    {
        entComment.Text = comment;
    }

    // Set date field.
    public void setDate(string date)
    {
        entDate.Text = date;
    }

    // Set priority field.
    public void setPrio(string prio)
    {
        entPrio.Text = prio;
    }
}
