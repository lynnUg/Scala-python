import org.python.util.PythonInterpreter  
import org.python.core.{Py, PyString,PyList,PySystemState}  
   
 object Program extends App{  
 val interp = new PythonInterpreter(null,new PySystemState()) 
 val sys = Py.getSystemState() 
 sys.path.append(new PyString("C:\\Users\\Lynn\\Documents\\GitHub\\todo-api\\lib\\Lib\\site-packages\\flask-0.10.1-py2.7.egg"))  
 sys.path.append(new PyString("C:\\Users\\Lynn\\Documents\\GitHub\\todo-api\\lib\\Lib\\site-packages\\setuptools-0.6c11-py2.7.egg ")) 
 sys.path.append(new PyString("C:\\Users\\Lynn\\Documents\\GitHub\\todo-api\\lib\\Lib\\site-packages")) 
 println(sys.path)
    
  val scriptFile = new java.io.FileInputStream("C:\\Users\\Lynn\\Documents\\Github\\todo-api\\src\\main\\python\\restserver.py")  
   
  val output=interp.execfile(scriptFile)
   //println(output)
  
   println("press any key to exit...")  
   readLine()  
 }  