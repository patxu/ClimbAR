from Tkinter import *
from tkFileDialog import askopenfilename
from PIL import Image, ImageTk

class Annotation(object):
    def __init__(self, startX=0, startY=0, boxId = -1):
        self.startX = startX
        self.startY = startY
        self.endX = 0
        self.endY = 0
        self.boxId = boxId

class Annotator(object):
    def __init__(self, currentBox=None):
        self.currentBox = currentBox

if __name__ == "__main__":
    root = Tk()

    #setting up a tkinter canvas with scrollbars
    frame = Frame(root, bd=2, relief=SUNKEN)
    frame.grid_rowconfigure(0, weight=1)
    frame.grid_columnconfigure(0, weight=1)
    xscroll = Scrollbar(frame, orient=HORIZONTAL)
    xscroll.grid(row=1, column=0, sticky=E+W)
    yscroll = Scrollbar(frame)
    yscroll.grid(row=0, column=1, sticky=N+S)
    canvas = Canvas(frame, bd=0, xscrollcommand=xscroll.set, yscrollcommand=yscroll.set)
    canvas.grid(row=0, column=0, sticky=N+S+E+W)
    xscroll.config(command=canvas.xview)
    yscroll.config(command=canvas.yview)
    frame.pack(fill=BOTH,expand=1)

    #adding the image
    File = askopenfilename(parent=root, title='Choose an image.')
    img = ImageTk.PhotoImage(Image.open(File))
    canvas.create_image(0,0,image=img,anchor="nw")
    canvas.config(scrollregion=canvas.bbox(ALL))


    #keep track of annotations
    annotations = {}
    boxes = []
    an = Annotator()


    def saveAnnotations():
        for box in boxes:
            print annotations[box]
        
    def clearAnnotations():
        for box in boxes:
            canvas.delete(box)
            del annotations[box]
            del boxes[box]

    def click(event):
        an.currentBox = canvas.create_rectangle(0,0,0,0)
        annotations[an.currentBox] = Annotation(canvas.canvasx(event.x), canvas.canvasy(event.y), an.currentBox)
        boxes.append(an.currentBox)

    def drag(event):
        canvas.coords(an.currentBox, annotations[an.currentBox].startX, annotations[an.currentBox].startY, canvas.canvasx(event.x), canvas.canvasy(event.y))

    def release(event):
        annotations[an.currentBox].endX = canvas.canvasx(event.x)
        annotations[an.currentBox].endY = canvas.canvasy(event.y)



    b = Button(root, text="Save Annotations", command=saveAnnotations)
    b.pack()
    b = Button(root, text="Clear Annotations", command=clearAnnotations)
    b.pack()

    canvas.bind("<Button-1>",click)
    canvas.bind("<B1-Motion>", drag)
    canvas.bind("<ButtonRelease-1>", release)
    root.mainloop()
