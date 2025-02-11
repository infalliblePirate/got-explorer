import { useLocation, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import "./MainPage.scss";
import InfoSection from "./InfoSection";
import Navigation from "../additional_components/Navigation";
import Footer from "../additional_components/Footer";
import TeamPage from "./TeamPage";
import GoToGame from "./GoToGame";
const MainPage = () => {
    const navigate = useNavigate(); 
    const location = useLocation();
    useEffect(() => {
        if (location.hash) {
            const element = document.querySelector(location.hash);
            if (element) {
                element.scrollIntoView({ behavior: 'smooth' });
            }
        }
    }, [location]);
    const handleGameClick = () => navigate("/startgame");

   return (
        <div className="page">
            <div className='backg-img'></div>
                <div className='wrapper'>
                    <Navigation />
                    <div className='titles'>    
                        <div className='title-font'>
                            when you play game of thrones
                        </div>
                        <div className='main-title-font'>
                            you win or you die
                        </div>
                        <button className='start-btn' onClick={handleGameClick}>
                            START NOW
                        </button>
                        
                    </div>
                </div>
            <img  className="main-2" />
            
            <img className='reverse-vector' />
            <div id="info-section">
                <InfoSection />
            </div>
            <div id="go-to-game">
                <GoToGame />
            </div>
            
            <div id="team-section">
                <TeamPage />
            </div>
            <Footer/>
        </div>
    );
}

export default MainPage;