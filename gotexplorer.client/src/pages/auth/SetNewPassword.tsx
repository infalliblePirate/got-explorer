/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from "react";
import { Navigate, useNavigate, useSearchParams } from "react-router-dom";
import Cookies from "universal-cookie";
import warning from "../../assets/images/warning.webp";
import "./Auth.scss";
import authService from "./authService";

const SetNewPasswordPage = () => {
    useEffect(() => {
        document.body.classList.add("auth-body");

        return () => {
            document.body.classList.remove("auth-body");
        };
    }, []);
    const fixToken = (token: string) =>
        token.replace(/ /g, '+');
    const [searchParams] = useSearchParams();
    const [params] = useState({
        id: parseInt(searchParams.get("id") as string),
        token: fixToken(searchParams.get("token") as string)
    });
    const PWD_REGEX = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%]).{8,24}$/;
    const [userPassword, setUserPassword] = useState({
        firstPass: "",
        secondPass: ""
    });
    const [showAlert, setShowAlert] = useState(false);
    const [errMsg, setErrMsg] = useState([""]);

    const [isPasswordVisible, setPasswordVisible] = useState({
        forFirstPass: false,
        forSecondPass: false
    });
    const togglePasswordVisibility = (fieldName: "forFirstPass" | "forSecondPass") => {
        setPasswordVisible((prev) => ({
            ...prev,
            [fieldName]: !prev[fieldName],
        }));
    };

    const cookies = new Cookies();
    const isAuthenticated = cookies.get('token') != null ? true : false;
    const authserv = authService;
    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setUserPassword({
            ...userPassword,
            [e.target.name]: value
        });
    };
    function ShowMsg(msgtext: string) {
        setErrMsg(errMsg => [...errMsg, msgtext]);
        setShowAlert(true);
    }

    function Submit() {
        setErrMsg([""]);
        setShowAlert(false);
        if (userPassword.firstPass === "" || userPassword.secondPass === "") {
            ShowMsg("Please fill both fields");
            return;
        }
        if (userPassword.firstPass !== userPassword.secondPass) {
            ShowMsg("Passwords are not the same");
            return;
        }
        const passValid = PWD_REGEX.test(userPassword.firstPass);
        if (!passValid) {
            ShowMsg("Password should contain 1 uppercase letter; 1 lowercase letter; 1 digit; 1 special symbol");
            return;
        }

        authserv.set_new_pass(params.id, userPassword.firstPass, params.token)
            .then(() => {
                alert("Password is changed!");
                navigate("/login");
            })
            .catch((error: any) => {
                const errmsgs = error.response.data.errors;
                errmsgs.forEach((msg: { errorMessage: string; }) => setErrMsg(errMsg => [...errMsg, msg.errorMessage]));
                setShowAlert(true);
            });

    }
    return (<>{isAuthenticated ? <Navigate to="/"></Navigate> :
        <div className="auth-grid">
            <img className="photo-bg" />
            <div className="col-2">
                <img className="logo" />
                {showAlert &&
                    <div className="warning-alert">
                        <img src={warning}></img>
                        <p>{errMsg}</p>
                    </div>
                }
                <div className="greeting">
                    Set a new password
                </div>
                <form>
                    <div className="for-reset-password">
                        <label>Password</label>
                        <div id="pass-container">
                            <input id="pass"
                                name="firstPass"
                                type={isPasswordVisible.forFirstPass ? 'text' : 'password'}
                                placeholder="Enter your new password"
                                required
                                autoComplete="off"
                                defaultValue={userPassword.firstPass}
                                onChange={handleChange} />
                            <button type="button" onClick={() => togglePasswordVisibility("forFirstPass")}>
                                <img id="pass-eye-img" />
                            </button>
                        </div>
                    </div>
                    <div className="for-reset-password">
                        <label>Confirm password</label>
                        <div id="pass-container">
                            <input id="pass"
                                name="secondPass"
                                type={isPasswordVisible.forSecondPass ? 'text' : 'password'}
                                placeholder="Re-enter password"
                                required
                                autoComplete="off"
                                defaultValue={userPassword.secondPass}
                                onChange={handleChange} />
                            <button type="button" onClick={() => togglePasswordVisibility("forSecondPass")}>
                                <img id="pass-eye-img" />
                            </button>
                        </div>
                    </div>

                    <input className="submit-btn" type="button" value="Reset password" onClick={Submit}></input>
                </form>
            </div>
        </div>
    }
    </>
    );
}
export default SetNewPasswordPage;