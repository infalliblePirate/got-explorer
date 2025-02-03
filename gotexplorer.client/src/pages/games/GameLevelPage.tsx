import { useEffect, useState } from "react";
import { Scene } from "../../assets/models/scene";
import { Map2d } from "../../assets/models/map2d";
import { GameLogic } from "../../assets/models/GameLogic";
import './GameLevelPage.scss';
import "../../../node_modules/leaflet/dist/leaflet.css";
import Leaderboard, { Player } from './LeaderBoard'; 
import { Link } from "react-router-dom";

const GameLevelPage = () => {
    const [showLeaderboard, setShowLeaderboard] = useState(false);
    const [players, setPlayers] = useState<Player[]>([]); 
    const [gameLogic, setGameLogic] = useState<GameLogic | null>(null);
    const [currentScore, setCurrentScore] = useState(0); 

    
    useEffect(() => {
        const container = document.getElementById("three");
        if (!container) {
            console.error("Container for 3D scene not found.");
            return;
        }

        // scene
        const scene = new Scene(container);
        scene.loadModel("/assets/the-wall-with-lights.glb");
        scene.loadBackground("/assets/panorama.jpg");
        scene.animate();

        // map2d
        const imageBounds: [[number, number], [number, number]] = [[0, 0], [1024, 720]];

        const containerId = 'map';
        const map = new Map2d("/assets/map2.JPG", imageBounds, containerId);
        
        const targetLocation = { lat: 770, lng: 360};
        const radius = 60;
        const newGameLogic = new GameLogic(map, targetLocation, radius);
        setGameLogic(newGameLogic); 
        setPlayers([
            { rank: 1, name: "Alice", score: 120 },
            { rank: 2, name: "Bob", score: 100 },
            { rank: 3, name: "Charlie", score: 90 },
        ]);
    }, []);

 const handleSubmitAnswer = () => {
    if (!gameLogic) {
        console.error("GameLogic не ініціалізований");
        return;
    }
    if (!gameLogic.hasMarker()) {
        alert("Please place a marker on the map before submitting your answer."); 
        return;
    }

    gameLogic.submitAnswer(); 

    const newScore = gameLogic.getScore(); 
    setCurrentScore(newScore); 

    setShowLeaderboard(true); 
};


  
    const handleRestartGame = () => {
        if (!gameLogic) {
            console.error("GameLogic не ініціалізований");
            return;
        }

        gameLogic.reset(); 
        setCurrentScore(0); 
        setShowLeaderboard(false); 
    };

     
    return (
        <div className="model-and-map-container" id="container">
            <div id="three-container">
                <div className="model-3d-container" id="three"></div>
            </div>
            <div id="map-container">
                <div className="map-2d-container" id="map"></div>
                <button className="submit-button" onClick={handleSubmitAnswer}>
                    Submit Answer
                </button>
            </div>
            {showLeaderboard && (
                <div className="modal-overlay">
                    <div className="modal">
                        <Leaderboard players={players} currentScore={currentScore}  />
                        <div className="modal-buttons">
                            <button
                                className="restart-button"
                                onClick={handleRestartGame}
                            >
                                Restart Game
                            </button>
                            <Link to="/" className="homepage-link">
                                <button className="close-button">
                                    Go to Homepage
                                </button>
                            </Link>
    
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};
export default GameLevelPage;