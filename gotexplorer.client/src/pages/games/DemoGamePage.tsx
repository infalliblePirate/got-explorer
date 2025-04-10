/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useRef, useState } from "react";
import { Scene } from "../../assets/models/scene";
import { Map2d } from "../../assets/models/map2d";
import { GameLogic } from "../../assets/models/GameLogic";
import "./GameLevelPage.scss";
import "../../../node_modules/leaflet/dist/leaflet.css";
import GameService from "./GameService";
import Cookies from "universal-cookie";
import { Link, useNavigate } from "react-router-dom";
import { toast } from 'sonner';

const UploadLevelModel = (scene: Scene, id: number, gameserv: typeof GameService, map: Map2d): GameLogic => {
    console.log(`level id: ${id}`);
    scene.clearScene();
    scene.changeCameraPosition(1000, 1000, 1000);
    gameserv.load_level(id).then((r) => {
        scene.loadModel(r.data.models[0].path);
    });
    scene.animate();
    return new GameLogic(map);
};

let counter = 0;

const DemoGamePage = () => {
    const navigate = useNavigate();
    const [gameLogic, setGameLogic] = useState<GameLogic | null>(null); 
    const [isMapExpanded, setIsMapExpanded] = useState(false);
    const [showEndGame, setShowEndGame] = useState(false);
    const gameserv = GameService;
    const scene = useRef<Scene>();
    const map = useRef<Map2d>();
    const cookies = new Cookies();
    const levels = cookies.get("dailyDemoIds");
    const hasDemoPlayed = cookies.get('demoPlayed');

    useEffect(() => {
        if (hasDemoPlayed && !levels) {
            navigate("/register");
            return;
        }
        if (levels === undefined) {
            navigate("/startgame");
        }

        const container = document.getElementById("three");
        if (!container) {
            console.error("Container for 3D scene not found.");
            return;
        }

        scene.current = new Scene(container);
        scene.current.loadBackground("/assets/panorama2.webp");

        const imageBounds: [[number, number], [number, number]] = [[0, 0], [1080, 720]];
        map.current = new Map2d("/assets/map2.webp", imageBounds, "map");

        if (scene.current && map.current) {
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
            toast.info("Please place a marker on the map before submitting your answer.");
            return;
        }

        counter += 1;
        setIsMapExpanded(false);

        if (counter < 3) {
            if (scene.current && map.current) {
                setGameLogic(UploadLevelModel(scene.current, levels[counter], gameserv, map.current));
            }
        } else {
            setShowEndGame(true);
            counter = 0;
        }
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
                        <button className="submit-button" onClick={handleSubmitAnswer}>Submit Answer</button>
                    </>
                )}
            </div>

            {showEndGame && (
                <div className="modal-overlay">
                    <div className="demo">
                        <h2>Demo Completed</h2>
                        <p>You have finished the demo game. To continue playing, please register or log in.</p>
                        <div className="modal-buttons">
                            <Link to="/register"><button className="close-button">Register</button></Link>
                            <Link to="/login"><button className="close-button">Log In</button></Link>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default DemoGamePage;
