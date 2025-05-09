import api from "../../services/api";
import Cookies from 'universal-cookie';
import { getAuthConfig } from "./GetAuthConfig";
import { jwtDecode } from "jwt-decode";
class AuthService {
    cookies = new Cookies(null, { path: '/' });

    login(username: string, password: string, rememberMe: boolean) {
        return api
            .post("/account/login", { username, password })
            .then((response) => {
                const token = response.data.token;
                const imageId = response.data.imageId;
                if (token) {
                    if (rememberMe) {
                        this.cookies.set("token", token, { path: "/", maxAge: 60 * 60 * 24 * 10 });
                        if (imageId) {
                            this.cookies.set("imageId", imageId, { path: "/", maxAge: 60 * 60 * 24 * 10 });
                        }
                    } else {
                        this.cookies.set("token", token, { path: "/" });
                        if (imageId) {
                            this.cookies.set("imageId", imageId, { path: "/" });
                        }
                    }
                }
                return response.data;
            });
    }

    logout() {
        this.cookies.remove('gameid');
        this.cookies.remove('levelIds');
        this.cookies.remove('token');
        this.cookies.remove('imageId');
        this.cookies.remove('changedName')
        this.cookies.remove('changedImage');
        this.cookies.remove('dailyLevelIds');
    }

    signup(username: string, email: string, password: string) {
        return api.post("/account/register", {
            username,
            email,
            password,
        }).then((response) => {
            const token = response.data.token;
            const imageId = response.data.imageId;
            if (token) {
                this.cookies.set('token', token);
            }
            if (imageId) {
                this.cookies.set('imageId', imageId)
            }
            return response.data;
        });
    }
    login_google(idToken: string) {
        return api.post("/account/login/google", {
            idToken
        }).then((response) => {
            const token = response.data.token;
            const imageId = response.data.imageId;
            if (token) {
                this.cookies.set('token', token);
            }
            if (imageId) {
                this.cookies.set('imageId', imageId)
            }
            return response.data;
        });
    }
    update(username: string, email: string) {
        const imageId = this.cookies.get("imageId");
        return api.put("/account/update", {
            username,
            email,
            imageId
        },
            {
                headers: { Authorization: `Bearer ${this.cookies.get('token')}` }
            });
    }
    update_photo() {
        const imageId = this.cookies.get("changedImage") || this.cookies.get("imageId");
        let username: string | null = null;
        let email: string | null = null;
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const decoded: any = jwtDecode(this.cookies.get("token"));
        username = decoded.username;
        email = decoded.email;
        return api.put("/account/update", {
            username,
            email,
            imageId
        },
            {
                headers: { Authorization: `Bearer ${this.cookies.get('token')}` }
            }).then(() => {
                if (this.cookies.get("changedImage") || this.cookies.get("imageId")) {
                    this.cookies.set("imageId", imageId);
                    this.cookies.remove("changedImage");
                }
            });
    }
    update_password(currentPassword: string, newPassword: string) {
        return api.put("/account/update-password", {
            currentPassword,
            newPassword
        }, getAuthConfig());

    }
    reset_password(email: string) {
        return api.put("/account/password-reset-link", {
            email
        });
    }
    set_new_pass(id: number, password: string, token: string) {
        return api.put("/account/password-reset", {
            id,
            password,
            token
        });
    }
    delete() {
        api.delete("/account/delete", getAuthConfig());
        this.logout();
    }
    get_images() {
        return api.get("/image")
            .then((r) => {
                return r.data;
            });
    }
    get_image(id: string) {
        return api.get(`/image/${id}`, {
            responseType: "blob",
        })
            .then((r) => {
                return URL.createObjectURL(r.data);
            });
    }
}

export default new AuthService();