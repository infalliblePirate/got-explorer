import { Link } from "react-router-dom";
import "../homepage/MainPage.scss";
import Cookies from "universal-cookie";
import { useEffect, useState } from "react";
import { useProfileImage } from "../auth/ImageContex";

const Navigation = () => {
    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null ? true : false;
    const [width, setWidth] = useState(window.innerWidth);
    const [toggleNav, setToggleNav] = useState(false);
    const { profileImage } = useProfileImage();

    useEffect(()=>{
        function handleResize() {
            setWidth(window.innerWidth);
          }
          window.addEventListener('resize', handleResize);
          return () => window.removeEventListener('resize', handleResize);
    },[]);

    const toggleNavFunc = () => {
        setToggleNav(!toggleNav);
      };

    return (<nav >
        <Link to="/">
            <img className="logo" />
        </Link>
        {width < 600 &&(
            <button className="hamburger-btn" onClick={toggleNavFunc}>
                ☰
            </button>
        )} 
        <button className="hamburger-btn" onClick={toggleNavFunc}>
                ☰
            </button>
        <div className={`menu-info ${toggleNav ? "active" : ""}`}>
            <ul>
                <li>
                    <Link to="/#info-section" className="link">
                        Explore the GOT world
                    </Link>
                </li>
                <li>
                    <Link to="/#team-section" className="link">
                        Team
                    </Link>
                </li>
                <li>
                    {isAuthenticated ? <>
                        <Link to="/profile">
                            <img src={profileImage ?? undefined} style={{ width: "48px", borderRadius: "50%"}}></img>
                        </Link>
                    </> :
                        <Link to="/login" className="link">
                            <button className='login-btn'>
                                Log in
                            </button>
                        </Link>
                    }
                </li>
            </ul>
        </div>
    </nav>);
}
export default Navigation;