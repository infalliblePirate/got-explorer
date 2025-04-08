import Cookies from "universal-cookie";
import Footer from "../additional_components/Footer";
import Navigation from "../additional_components/Navigation";
import GameService from "./GameService";
import './StartGamePage.scss';
import { Navigate, useNavigate } from "react-router-dom";
import { toast } from 'sonner';
import ErrorHandle from "../../utils/ErrorHandle";

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
                    toast.error(`Couldn't start the game: ${ErrorHandle(error.response.data.errors)}`, {
                        style: {
                            backgroundColor: '#5d8ecf',
                            color: 'white'
                        }
                    });
                });
            return;
        }
        navigate("/lvl/game");
    };
    const handleDailyGameClick = () => {
        if (!isAuthenticated) {
            navigate("/login");
            return;
        }

        if (!cookies.get("dailyGameId")) {
            gameserv.start_daily_game()
                .then(() => {
                    navigate("/lvl/dailygame");
                })
                .catch((error) => {
                    if (error.response?.status === 409) {
                        toast.error("You have already played today's game!", {
                            style: {
                                backgroundColor: '#5d8ecf',
                                color: 'white'
                            }
                        });
                    } else {
                        toast.error(`Error starting daily game: ${ErrorHandle(error.response.data.errors)}`, {
                            style: {
                                backgroundColor: '#5d8ecf',
                                color: 'white'
                            }
                        });
                    }
                });
            return;
        }
        navigate("/lvl/dailygame");
    };
    return (
        <>{isAuthenticated ?
            <div className="start-game-page">
                <Navigation />
                <div className="main-content">
                    <div className="text">LET'S EXPLORE THE WORLD</div>
                    <div className="buttons">
                        <button className="button-start" onClick={handleGameClick}>STANDART</button>
                        <button className="button-start" onClick={handleDailyGameClick}>DAILY GAME</button>
                    </div>
                </div>
                <Footer />
            </div > :
            <Navigate to="/login"></Navigate>}
        </>);
}

export default StartGamePage;