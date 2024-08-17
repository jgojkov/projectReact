import {React} from "react";
import Navbar from "./Navbar";
import Footer from "./Footer";
import Breadcrumb from "./Breadcrumb";
import { Outlet } from "react-router-dom";

const LayoutWrapper = ({ children }) => {
  return (
    <>
      <Navbar/>
      <Breadcrumb />
      <main>
        <Outlet/>
      </main>
      <div className="container-fluid">{children}</div>
      <Footer />
    </>
  );
};

export default LayoutWrapper; 
