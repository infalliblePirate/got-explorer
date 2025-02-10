import Footer from "../additional_components/Footer";
import Navigation from "../additional_components/Navigation";
import './StartGamePage.scss';
import { useNavigate } from "react-router-dom";

const StartGamePage = () => {

    const navigate = useNavigate();
    const handleGameClick = () => navigate("/lvl/game");
    return (<div className="start-game-page">
             <Navigation />
             <div className="main-content">
                 <div className="text">LET'S EXPLORE THE WORLD</div>
                 <button className="button-start" onClick={handleGameClick}>START NOW</button>
             </div>

             <Footer />
         </div>);
}

export default StartGamePage;