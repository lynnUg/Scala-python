from flask import Flask ,jsonify 
from ij import IJ
app = Flask(__name__) 


@app.route('/image/<image>')
def show_user_profile(image):
    # show the user profile for that user
    return image
@app.route('/')  
def hello():  
   IJ.showMessage("Hello World!")
   return 'Hello World!'     
@app.errorhandler(404)  
def page_not_found(e):  
   return 'Sorry, Nothing at this URL.', 404  
@app.errorhandler(500)  
def page_not_found(e):  
   return 'Sorry, unexpected error: {}'.format(e), 500  
   
if __name__ == "__main__":  
 	app.run()  