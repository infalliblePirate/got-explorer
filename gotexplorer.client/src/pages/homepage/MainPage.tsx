import { useLocation } from "react-router-dom";
import { useEffect } from "react";
import "./MainPage.scss";
import InfoSection from "./InfoSection";
import Navigation from "../additional_components/Navigation";
import Footer from "../additional_components/Footer";
import TeamPage from "./TeamPage";
import GoToGame from "./GoToGame";
const MainPage = () => {

    const location = useLocation();
    useEffect(() => {
        if (location.hash) {
            const element = document.querySelector(location.hash);
            if (element) {
                element.scrollIntoView({ behavior: 'smooth' });
            }
        }
    }, [location]);

    return (
        <div className="page">
            <div className='backg-img'>
                <div className='wrapper'>
                    <Navigation />
                    <div className='titles'>
                        <div className='title-font'>
                            when you play game of thrones
                        </div>
                        <div className='main-title-font'>
                            you win or you die
                        </div>
                        <GoToGame/>
                    </div>
                    <img className='dragon-img' />
                    <img className='fire-img' />
                    <img className='bg-img' />
                    <img className='vector' />
                </div>
            </div>
            <img className='reverse-vector' />
            <div id="info-section">
                <InfoSection />
            </div>
            <div id="team-section">
                <TeamPage />
            </div>
            <Footer/>
        </div>
    );
}

export default MainPage;