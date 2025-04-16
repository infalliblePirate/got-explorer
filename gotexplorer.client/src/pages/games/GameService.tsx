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
        return api.put(`/game/${gameid}/calculatescore`, {
            levelId,
            x,
            y
        }, getAuthConfig());
    }
    calculate_level_daily(levelId: number, x: number, y: number) {
        const gameid = this.cookies.get("dailyGameId");
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
        return api.get("/leaderboard?OrderBy=Desc", getAuthConfig());
    }
    start_daily_game() {
        return api
            .post("/game/start/daily", null, getAuthConfig())
            .then((response) => {
                this.cookies.remove("dailyGameId");
                this.cookies.set("dailyGameId", response.data.gameId);
                this.cookies.set("dailyLevelIds", response.data.levelIds);
                return response.data;
            })
            .catch((error) => {
                throw error;
            });
    }
    complete_daily_game() {
        const dailyGameId = this.cookies.get("dailyGameId");
        return api.put(`/game/${dailyGameId}/complete/daily`, null, getAuthConfig())
            .then((response) => {
                this.cookies.remove("dailyGameId");
                this.cookies.remove("dailyLevelIds");
                return response.data;
            })
            .catch((error) => {
                throw error;
            });
    }
    getLeaderboardDaily() {
        return api.get("/leaderboard?GameType=Daily&OrderBy=Desc", getAuthConfig());
    }
    start_demo_game() {
        if (this.cookies.get('demoPlayed')) {
            return Promise.reject({ message: "Demo already played" });
        }
        return api
            .post("/game/start/demo", null, getAuthConfig())
            .then((response) => {
                this.cookies.remove("dailyDemoIds");
                this.cookies.set("dailyDemoIds", response.data.levelIds);
                return response.data;
            })
            .catch((error) => {
                throw error;
            });
    }

}
export default new GameService();