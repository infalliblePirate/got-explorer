import { Link } from "react-router-dom";
import "../homepage/MainPage.scss"
const Navigation = () => {
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
                    <Link to="/login" className="link">
                        <button className='login-btn'>
                            Log in
                        </button>
                    </Link>
                </li>
            </ul>
        </div>
    </nav>);
}
export default Navigation;