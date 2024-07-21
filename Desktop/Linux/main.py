import gi
gi.require_version('Gtk', '4.0')
from gi.repository import Gtk, GLib, Gio
import subprocess
import threading
class MyDialog(Gtk.Dialog):
    def __init__(self, parent):
        super().__init__(title="VedAstro Desktop", transient_for=parent, modal=True)
        self.set_default_size(400, 300)
        content_area = self.get_content_area()
        main_box = Gtk.Box(orientation=Gtk.Orientation.VERTICAL, spacing=10)
        content_area.append(main_box)
        title_label = Gtk.Label(label="Open + Powerful + Easy")
        main_box.append(title_label)
        self.output_textview = Gtk.TextView()
        scrolled_window = Gtk.ScrolledWindow()
        scrolled_window.set_child(self.output_textview)
        main_box.append(scrolled_window)
        run_button = Gtk.Button(label="Run Command")
        run_button.connect("clicked", self.run_command)
        main_box.append(run_button)
        self.show_all()
    def run_command(self, button):
        command = ["ls", "-l"]  # Replace with your desired command
        def update_text(process):
            for line in iter(process.stdout.readline, b''):
                GLib.idle_add(self.output_textview.get_buffer().insert_at_cursor, line.decode())
        process = subprocess.Popen(command, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
        threading.Thread(target=update_text, args=(process,)).start()
class MyApp(Gtk.Application):
    def __init__(self):
        super().__init__(application_id="com.example.vedastro")
    def do_activate(self):
        window = Gtk.ApplicationWindow(application=self)
        window.set_title("VedAstro Main Window")
        window.set_default_size(600, 400)
        button = Gtk.Button(label="Open Dialog")
        button.connect("clicked", self.open_dialog, window)
        window.set_child(button)
        window.present()
    def open_dialog(self, button, parent):
        dialog = MyDialog(parent)
        dialog.run()
        dialog.destroy()
app = MyApp()
app.run(None)