import numpy as np
import cv2

hold_cascade = cv2.CascadeClassifier('train_cascade/cascade.xml')

img = cv2.imread('img/test/63out.jpg')
gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

holds = hold_cascade.detectMultiScale(gray, 1.3, 5)
print holds
for (x,y,w,h) in holds:
    cv2.rectangle(img,(x,y),(x+w,y+h),(255,0,0),2)
    roi_gray = gray[y:y+h, x:x+w]
    roi_color = img[y:y+h, x:x+w]

cv2.imwrite('detected.jpg',img)
# cv2.waitKey(0)
# cv2.destroyAllWindows()
