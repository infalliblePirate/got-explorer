import Cookies from "universal-cookie";
import Footer from "../additional_components/Footer";
import Navigation from "../additional_components/Navigation";
import GameService from "./GameService";
import './StartGamePage.scss';
import { Navigate, useNavigate } from "react-router-dom";

const StartGamePage = () => {
    const gameserv = GameService;
    const navigate = useNavigate();
    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null ? true : false;

    const handleGameClick = () => {
        if (!isAuthenticated) {
            navigate("/login");
            return;
        }

        if (!cookies.get("gameid")) {
            gameserv.start_game()
                .then(() => {
                    navigate("/lvl/game");
                })
                .catch((error) => {
                    console.log(error);
                });
            return;
        }
        navigate("/lvl/game");
    };
    return (
        <>{isAuthenticated ?
            <div className="start-game-page">
                <Navigation />
                <div className="main-content">
                    <div className="text">LET'S EXPLORE THE WORLD</div>
                    <button className="button-start" onClick={handleGameClick}>START NOW</button>
                </div>

                <Footer />
            </div > :
            <Navigate to="/login"></Navigate>}
        </>);
}

export default StartGamePage;