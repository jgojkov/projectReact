import React from "react";
import { Link } from "react-router-dom";
import '../Style/links.css';

const Breadcrumb = () => (
      <div className="links">
        <Link to="/">Početna&nbsp;</Link>
        <Link to="/dataExport">Podaci&nbsp;</Link>
        <Link to="/contact">Kontakt</Link>
      </div>
);

export default Breadcrumb;