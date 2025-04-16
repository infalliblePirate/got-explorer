import Cookies from "universal-cookie";
import Footer from "../additional_components/Footer";
import Navigation from "../additional_components/Navigation";
import GameService from "./GameService";
import './StartGamePage.scss';
import { useNavigate } from "react-router-dom";
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
                    toast.error(`Couldn't start the game: ${ErrorHandle(error.response.data.errors)}`);
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
                    if (error.response?.status === 429) {
                        toast.error("You have already played today's game!");
                    } else {
                        toast.error(`Couldn't start the game: ${ErrorHandle(error.response.data.errors)}`);
                    }
                });
            return;
        }
        navigate("/lvl/dailygame");
    };
    
    const handleDemoGameClick = () => {
        const hasDemoPlayed = cookies.get('demoPlayed');
    
        if (hasDemoPlayed) {
            toast.info("You've already played our demo game. Please register to continue playing!");
            navigate("/login");
            return;
        }
        
        gameserv.start_demo_game()
            .then(() => {
                cookies.set('demoPlayed', 'true', { maxAge: 365 * 24 * 60 * 60 });
                navigate("/lvl/demogame");
            })
            .catch((error) => {
                toast.error(`Failed to start demo game: ${ErrorHandle(error.response.data.errors)}`);
            });
    };
    return (
        <>
            <div className="start-game-page">
                <Navigation />
                <div className="main-content">
                    <div className="text">LET'S EXPLORE THE WORLD</div>
                    {isAuthenticated ? (
                        <div className="buttons">
                            <button className="button-start" onClick={handleGameClick}>STANDARD</button>
                            <button className="button-start" onClick={handleDailyGameClick}>DAILY GAME</button>
                        </div>
                    ) : (
                        <div className="buttons">
                            <button className="button-start" onClick={handleDemoGameClick}>START DEMO GAME</button>
                        </div>
                    )}
                </div>
                <Footer />
            </div > 

        </>);
}

export default StartGamePage;