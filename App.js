import React from "react";
import { BrowserRouter,
         Routes,
         Route,
 } from "react-router-dom";

  
  import Home from "./Components/Home/Home.js";
  import DataExport from "./Components/DataExport/DataExport.js";
  import Contact from "./Components/Contact/Contact.js";
  import LayoutWrapper from "./Components/Layout/LayoutWrapper.js";
  
  function App() {
    return (
    <BrowserRouter>
       <Routes>
         <Route element={<LayoutWrapper />} >
           <Route path="/" element={<Home />} />
           <Route path="dataExport" element={<DataExport />} />
           <Route path="contact" element={<Contact />} />
         </Route>
       </Routes>
     </BrowserRouter>
  
    )
    
  }
  
  export default App; 