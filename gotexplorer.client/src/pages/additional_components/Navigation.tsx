import { Link } from "react-router-dom";
import "../homepage/MainPage.scss";
import profile from "../../assets/images/profile_img.png";
import Cookies from "universal-cookie";
const Navigation = () => {
    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null ? true : false;

    return (<nav>
        <Link to="/">
            <img className="logo" />
        </Link>
        <div className='menu-info'>
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
                            <img src={profile} style={{ width: "48px" }}></img>
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