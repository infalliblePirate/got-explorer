import api from "../../services/api";
import Cookies from "universal-cookie";
import { getAuthConfig } from "../auth/GetAuthConfig";

class GameService {
    cookies = new Cookies(null, { path: '/' });
    config = {
        headers: { Authorization: `Bearer ${this.cookies.get('token')}` }
    };
    start_game() {
        return api
            .post("/game/start", null, getAuthConfig())
            .then((response) => {
                this.cookies.remove("gameid");
                this.cookies.set("gameid", response.data.gameId);
                this.cookies.set("levelIds", response.data.levelIds);
                return response.data;
            });
    }
    load_level(id: number) {
        return api.get(`/level/${id}`);
    }
    calculate_level(levelId: number, x: number, y: number) {
        const gameid = this.cookies.get("gameid");
        console.log(gameid);
        return api.put(`/game/${gameid}/calculatescore`, {
            levelId,
            x,
            y
        }, getAuthConfig());
    }
    complete_game() {
        const gameid = this.cookies.get("gameid");
        return api.put(`/game/${gameid}/complete`,
            null,
            getAuthConfig()).then((response) => {
                this.cookies.remove("gameid");
                this.cookies.remove("levelIds");
                return response.data;
            });
    }
    getLeaderboard() {
        console.log("Making GET request to leaderboard");
        return api.get("/leaderboard?OrderBy=Asc", getAuthConfig());
    }
}
export default new GameService();