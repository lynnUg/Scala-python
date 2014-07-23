from flask import Flask ,jsonify, request,send_file
from ij import IJ,ImagePlus,Macro
from java.util import Random
import jarray
#import Image
#from PIL import Image
from java.io import ByteArrayInputStream, File
from ij.io import TiffDecoder,Opener,FileSaver
from ij.process import ByteProcessor,LUT
from java.lang import String as javaString 
from jarray import array, zeros 
from javax.imageio import ImageIO
from ij.plugin.frame import RoiManager
import binascii
app = Flask(__name__) 


@app.route('/image/', methods=['POST'])
def receive_image():
	print "recieving an image ....."
	the_data=request.data
	fh = open("imageToSave.jpg", "wb")
	fh.write(the_data)
	fh.close()
	imp=IJ.openImage("C:/Users/Lynn/Documents/GitHub/server/imageToSave.jpg")
	IJ.run(imp,"8-bit","")
	IJ.run(imp,"Subtract Background...", "rolling=100 light disable")
	IJ.setAutoThreshold(imp,"Default")
	IJ.setAutoThreshold(imp,"Triangle")
	IJ.setThreshold(imp,0, 237)
	#Macro.setOptions("BlackBackground")
	IJ.run(imp, "Convert to Mask","")
	IJ.run(imp, "Convert to Mask","")
	IJ.run(imp, "Make Binary","")
	IJ.run(imp,"Fill Holes","")
	IJ.run(imp,"Watershed","")
	#IJ.run(imp,"Analyze Particles...", "size=2000-20000 display clear summarize add")
	IJ.run(imp,"Analyze Particles...", "size=6000-18000 circularity=0.07-0.25 show=[Overlay Outlines] display clear summarize")
	imp.show()
	FileSaver(imp).saveAsJpeg("C:/Users/Lynn/Documents/GitHub/server/imageToSave3.jpg")
	with open("imageToSave3.jpg", "rb") as imageFile:
		to_be_sent_str = imageFile.read()
	#print imp
	print "done"
	#RoiManager.runCommand("Show All with labels")
	#RoiManager.runCommand("Show All")
	#return "other"
	return to_be_sent_str
@app.route('/')  
def hello():  
   IJ.showMessage("Hello World!")
   imp = IJ.createImage("A Random Image", "8-bit", 512, 512, 1)
   Random().nextBytes(imp.getProcessor().getPixels())
   imp.show()
   return 'Hello World!'     
@app.errorhandler(404)  
def page_not_found(e):  
   return 'Sorry, Nothing at this URL.', 404  
@app.errorhandler(500)  
def page_not_found(e): 
	print "the error"
	print e
	return 'Sorry, unexpected error: {}'.format(e), 500  
   
if __name__ == "__main__":  
 	app.run(host= '0.0.0.0')
