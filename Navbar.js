import { Link } from "react-router-dom";
import logo from "../Logos/logo.png";
import '../Style/main-menu.css';

function Navbar() {
  return (
   
        <div className="jum-header">
          <div className="jum-header-title">
            <Link id="logoLink" to="/">
              <img
                style={{ marginLeft: "5px"}}
                id="logo"
                src={logo}
                alt="Logo"
              />
            </Link>
            <span
              className=""
              style={{
                padding: "10px 70px",
                fontSize: "xx-large",
                color: "red",
              }}
            >
              GSB reporting site
            </span>
          </div>

        </div>
    
  );
}

export default Navbar;