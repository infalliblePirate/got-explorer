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
import ErrorHandle from "../../utils/ErrorHandle";
import { toast } from "sonner";

const UploadLevelModel = (scene: Scene, id: number, gameserv: typeof GameService, map: Map2d): GameLogic => {
    scene.clearScene();
    scene.changeCameraPosition(1000, 1000, 1000);
    gameserv.load_level(id).then((r) => {
        scene.loadModel(r.data.models[0].path);
    })
    scene.animate();
    return new GameLogic(map);
}
// eslint-disable-next-line prefer-const
let counter = 0;
const DailyGamePage = () => {
    const navigate = useNavigate();
    const [showLeaderboard, setShowLeaderboard] = useState(false);
    const [players, setPlayers] = useState<Player[]>([]);
    const [gameLogic, setGameLogic] = useState<GameLogic | null>(null);
    const [currentScore, setCurrentScore] = useState(0);
    const [isMapExpanded, setIsMapExpanded] = useState(false);
    const [currentUserId, setCurrentUserId] = useState<number | null>(null);

    const gameserv = GameService;
    const scene = useRef<Scene>();
    const map = useRef<Map2d>();
    const map2dRef = useRef<Map2d | null>(null); 
    const cookies = new Cookies();
    const levels = cookies.get("dailyLevelIds");

    useEffect(() => {

        if (levels === undefined) {
            navigate(0);
        }

        const container = document.getElementById("three");
        if (!container) {
            toast.error("Container for 3D scene not found.", {
                style: {
                    backgroundColor: '#5d8ecf',
                    color: 'white'
                }
            });
            return;
        }

        // scene
        scene.current = new Scene(container);
        if (scene != undefined) {
            scene.current.loadBackground("/assets/panorama2.webp");
        }
        // map2d
        const imageBounds: [[number, number], [number, number]] = [[0, 0], [1080, 720]];

        const containerId = 'map';
        const newMap = new Map2d("/assets/map2.webp", imageBounds, containerId);
        map.current = newMap;
        map2dRef.current = newMap;

        if (scene.current !== undefined) {
            setGameLogic(UploadLevelModel(scene.current, levels[counter], gameserv, newMap));
        }

        return () => {
        };
    }, []);
    useEffect(() => {
      if (map2dRef.current) {
        setTimeout(() => {
          map2dRef.current!.handleContainerResize();
        }, 300);
      }
    }, [isMapExpanded]);

    const toggleMap = () => {
        setIsMapExpanded((prev) => !prev);
    };

    const handleSubmitAnswer = async () => {
        if (!gameLogic) {
            toast.error("GameLogic is not initialized", {
                style: {
                    backgroundColor: '#5d8ecf',
                    color: 'white'
                }
            });
            return;
        }
        if (!gameLogic.hasMarker()) {
            toast.info("Please place a marker on the map before submitting your answer.");
            return;
        }
        const click = gameLogic.getClick();
        if (click != null) {
             gameserv.calculate_level_daily(levels[counter], Math.round(click.lat * 100) / 100, Math.round(click.lng * 100) / 100);
        }
      
        setIsMapExpanded(false);
     
        try {
            const completedGame = await gameserv.complete_daily_game();
            const response = await gameserv.getLeaderboardDaily();
            const currentUserId = completedGame.userId;
            setCurrentUserId(currentUserId);
            const leaderboardData: Player[] = response.data.map((playerData: any) => ({
                userId: playerData.userId,
                username: playerData.username,
                score: playerData.score,
                startTime: playerData.startTime,
                endTime: playerData.endTime,
            }));
            setPlayers(leaderboardData);
            if (completedGame && completedGame.score !== undefined) {
                setCurrentScore(completedGame.score);
            }
            
            setShowLeaderboard(true);
        } catch (error : any) {
            toast.error(`Error fetching leaderboard: ${ErrorHandle(error.response.data.errors)}`, {
                style: {
                    backgroundColor: '#5d8ecf',
                    color: 'white'
                }
            });
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
                          <button className="submit-button" onClick={handleSubmitAnswer}>
                            Submit Answer
                        </button>
                    </>
                )}

                
            </div>

            {showLeaderboard && (
                <div className="modal-overlay">
                    <div className="modal">
                        <Leaderboard players={players} currentScore={currentScore} currentUserId={currentUserId}/>
                        <div className="modal-buttons">
                            <Link to="/" className="homepage-link">
                                <button className="close-button"  >
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

export default DailyGamePage;
