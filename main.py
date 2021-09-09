
import Application


class Main:
    def main(self):
        Application.DropCollection()  # Drop the prev collections
        Application.root.mainloop()  # Start the application's window


if __name__ == '__main__':
    Main().main()


