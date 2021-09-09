from tkinter import *
from connectDB import *
import rename


class Application(Frame):
    def __init__(self, master=None):
        Frame.__init__(self, master)
        # Path label
        self.pathLbl = Label(text="Path:").place(x=200, y=10)
        # Path input
        self.pathInput = Text(self.master, height=2, width=20)
        self.pathInput.place(x=200, y=50)
        # OldName label
        self.oldLbl = Label(text="File's name for change ").place(x=200, y=90)
        # OldName input
        self.oldNameInput = Text(self.master, height=2, width=20)
        self.oldNameInput.place(x=200, y=130)
        self.newLbl = Label(text=" Change To:").place(x=200, y=170)
        # newName label
        self.newNameInput = Text(self.master, height=2, width=20)
        # newName input
        self.newNameInput.place(x=200, y=210)
        # Submit button
        self.submit = Button(self.master,
                             text="Do That!",
                             command=self.on_click).place(x=200, y=250)
        # ACKlbl - Displays a response to the action.
        self.ACKlbl = Label(self.master, text="", fg="green")
        self.ACKlbl.place(x=200, y=300)
        self.ctrzLbl = Label(self.master, text="Press 'ctrl-z' to cancel the last action", font=(None, 13)).place(x=200,
                                                                                                                    y=350)
        # historyLbl - Displays the last action.
        self.historyLbl = Label(self.master, text="The last action:\n" + "no action", borderwidth=2, relief="groove",
                                font=(None, 13),
                                justify=LEFT)
        self.historyLbl.place(x=200, y=370)

    def on_click(self):
        """ Occurs when self.submit is clicked.
        Calls to rename function. """

        path = self.pathInput.get(0.1, "end-1c")
        oldName = self.oldNameInput.get(0.1, "end-1c")
        newName = self.newNameInput.get(0.1, "end-1c")
        isChange = rename.renameFile(oldName, newName, path)
        # Add the action to the DB
        if isChange:
            addToDB(oldName, newName, path)
        self.setHistoryLbl()  # Set history label

    def setHistoryLbl(self):
        """ Updates the history label to display the last action"""

        last = getLastFromDB()
        text = "The last action:\n" + "no action"
        if last:
            text = "The last action:\n" \
                   + "old name:  " + last['oldname'] + "\n" \
                   + "new name:  " + last['newname'] + "\n" \
                   + "path:  " + last['path']
        self.historyLbl.config(text=text)


def key(event=None):
    """ Occurs when ctl-z is pressed"""
    try:
        file = getLastFromDB()
        removeLastFromDB()
        rename.renameFile(file["newname"], file["oldname"], file["path"])
        app.setHistoryLbl()
        print("success")
    except:
        print("failed")


root = Tk()
root.geometry('600x500')
root.bind('<Control-z>', key)
app = Application(master=root)
