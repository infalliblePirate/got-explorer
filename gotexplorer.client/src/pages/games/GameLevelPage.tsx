/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useRef, useState } from "react";
import { Scene } from "../../assets/models/scene";
import { Map2d } from "../../assets/models/map2d";
import { GameLogic } from "../../assets/models/GameLogic";
import './GameLevelPage.scss';
import "../../../node_modules/leaflet/dist/leaflet.css";
import GameService from "./GameService";
import Cookies from "universal-cookie";
import { Link, useNavigate } from "react-router-dom";
import Leaderboard, { Player } from "./LeaderBoard";

const UploadLevelModel = (scene: Scene, id: number, gameserv: typeof GameService, map: Map2d): GameLogic => {
    console.log(`level id: ${id}`);
    console.log(scene);
    scene.clearScene();
    scene.changeCameraPosition(1000, 1000, 1000);
    gameserv.load_level(id).then((r) => {
        scene.loadModel(r.data.models[0].path);
    })
    scene.animate();
    return new GameLogic(map);
}
let counter = 0;
const GameLevelPage = () => {
    const navigate = useNavigate();
    const [showLeaderboard, setShowLeaderboard] = useState(false);
    const [players, setPlayers] = useState<Player[]>([]);
    const [gameLogic, setGameLogic] = useState<GameLogic | null>(null);
    const [currentScore, setCurrentScore] = useState(0);
    const [isMapExpanded, setIsMapExpanded] = useState(false);

    const gameserv = GameService;
    const scene = useRef<Scene>();
    const map = useRef<Map2d>();
    const cookies = new Cookies();
    const levels = cookies.get("levelIds");

    useEffect(() => {
        if (cookies.get("gameid") === undefined) {
            navigate("/startgame");
        }
        if (levels === undefined) {
            navigate(0);
        }

        const container = document.getElementById("three");
        if (!container) {
            console.error("Container for 3D scene not found.");
            return;
        }

        // scene
        scene.current = new Scene(container);
        if (scene != undefined) {
            scene.current.loadBackground("/assets/panorama2.webp");
        }
        console.log(scene);
        // map2d
        const imageBounds: [[number, number], [number, number]] = [[0, 0], [1080, 720]];

        const containerId = 'map';
        map.current = new Map2d("/assets/map2.webp", imageBounds, containerId);
        if (scene != undefined) {
            console.log(` levels are ${levels}`);
            setGameLogic(UploadLevelModel(scene.current, levels[counter], gameserv, map.current));
        }
    }, []);

    const toggleMap = () => {
        setIsMapExpanded(!isMapExpanded);
    };

    const handleSubmitAnswer = async () => {
        if (!gameLogic) {
            console.error("GameLogic is not initialized");
            return;
        }
        if (!gameLogic.hasMarker()) {
            alert("Please place a marker on the map before submitting your answer.");
            return;
        }
        if (counter != 3) {
            console.log(gameLogic.getClick());
            const click = gameLogic.getClick();
            console.log(click);
            if (click != null) {
                const r = await gameserv.calculate_level(levels[counter], Math.round(click.lat * 100) / 100, Math.round(click.lng * 100) / 100);
                console.log(r);
            }
            counter += 1;
        }
        setIsMapExpanded(!isMapExpanded);
        if (counter < 3) {
            if (scene.current != undefined && map.current != undefined)
                setGameLogic(UploadLevelModel(scene.current, levels[counter], gameserv, map.current));
        }
        else {
            try {
                counter = 0;
                const completedGame = await gameserv.complete_game();
                console.log(completedGame);
                console.log("Fetching leaderboard from API...");
                const response = await gameserv.getLeaderboard();


                const leaderboardData: Player[] = response.data.map((playerData: any) => ({
                    userId: playerData.userId,
                    username: playerData.username,
                    score: playerData.score,
                    startTime: playerData.startTime,
                    endTime: playerData.endTime,
                }));
                console.log("Leaderboard response:", response.data);
                setPlayers(leaderboardData);
                if (completedGame && completedGame.score !== undefined) {
                    setCurrentScore(completedGame.score);
                }
                
                setShowLeaderboard(true);
            } catch (error) {
                console.error("Error fetching leaderboard:", error);
            }
        }
    };
    const handleRestartGame = () => {
        if (!gameLogic) {
            console.error("GameLogic is not initialized");
            return;
        }

        gameLogic.reset();
        setCurrentScore(0);
        setShowLeaderboard(false);
        gameserv.start_game().then(() => {
        navigate(0);

        });
    };
      return (
        <div className="model-and-map-container" id="container">
            <div id="three-container">
                <div className="model-3d-container" id="three"></div>
            </div>

            <div id="map-container">
                <div className={`map-2d-container ${isMapExpanded ? "expanded" : "small"}`} id="map" onClick={!isMapExpanded ? toggleMap : undefined}></div>

                {isMapExpanded && (
                    <>
                        <button className="close-map-button" onClick={toggleMap}>✖️</button>
                          <button className="submit-button" onClick={handleSubmitAnswer}>
                            Submit Answer
                        </button>
                    </>
                )}

                
            </div>

            {showLeaderboard && (
                <div className="modal-overlay">
                    <div className="modal">
                        <Leaderboard players={players} currentScore={currentScore} />
                        <div className="modal-buttons">
                            <button
                                className="restart-button"
                                onClick={handleRestartGame}
                            >
                                Restart Game
                            </button>
                            <Link to="/" className="homepage-link">
                                <button className="close-button"  onClick={handleRestartGame}>
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
