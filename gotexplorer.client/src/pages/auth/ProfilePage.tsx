/* eslint-disable @typescript-eslint/no-explicit-any */
import Footer from '../additional_components/Footer';
import Navigation from '../additional_components/Navigation';
import './ProfilePage.scss';
import profileIcon from '../../assets/images/profile_img.webp';
import Cookies from 'universal-cookie';
import { Navigate, useNavigate } from 'react-router-dom';
import { jwtDecode, JwtPayload  } from 'jwt-decode';
import { useEffect, useState } from 'react';
import authService from './authService';
import GameService from '../games/GameService';
import { LeaderboardUser } from '../games/LeaderBoard';

const ProfilePage = () => {
        interface DecodedToken extends JwtPayload {
        name: string;
        email: string;
    }
    const cookies = new Cookies();
    const token = cookies.get('token');
    const isAuthenticated = token != null ? true : false;
    const authserv = authService;
    const navigate = useNavigate();
    const [userData, setUserData] = useState<DecodedToken>({
        name: "",
        email: ""
    });
    const [changedData, setChangedData] = useState({
        changedName: "",
        changedPassword:""
    });

    const [leaderboard, setLeaderboard] = useState<LeaderboardUser[]>([]);
    const [userRank, setUserRank] = useState<number | null>(null);
    const [userScore, setUserScore] = useState(0);
    const gameserv = GameService;
    
   useEffect(() => {
        try {
            const decoded = jwtDecode<DecodedToken>(token);
            setUserData({
                name: decoded.name,
                email: decoded.email 
            });

            gameserv.getLeaderboard()
                .then(response => {
                    const data: LeaderboardUser[] = response.data; 
                    setLeaderboard(data);

                    const userIndex = data.findIndex((user: LeaderboardUser) => user.username === decoded.name);
                    if (userIndex !== -1) {
                        setUserRank(userIndex + 1);
                        setUserScore(data[userIndex].score);
                    }
                })
                .catch(error => console.error("Error fetching leaderboard:", error));
        } catch (error) {
            console.error('Token decoding failed:', error);
        }
    }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setChangedData({
            ...changedData,
            [e.target.name]: value
        });
    };
    function changeProfile() {
        if (changedData.changedName != "") {
            authserv.update(changedData.changedName, userData.email)
                .catch((err) => {
                    console.error(err);
                });
        }
    }
    function deleteProfile() {
        try {
            authserv.delete();
            navigate("/");
        }
        catch (error) {
            console.log(error);
        }
    }
    function logOut() {
        authserv.logout();
        navigate("/");
    }
    return (<>{isAuthenticated ?
        <div className="profile-page">
            <Navigation />
            <div className="profile-container">
                <div className="email">
                    {userData ? (
                        <p>{userData.email}</p>
                    )
                        : (
                            <p>No email</p>
                        )
                    }
                </div>
                <div className="profile-header">
                    <div className="profile-picture-wrapper">
                        <img
                            className="profile-picture"
                            src={profileIcon}
                            alt="Profile"
                        />
                        <button className="change-picture-button">📸</button>
                    </div>
                    <h2 className="username">{userData.name}</h2>
                </div>

                <div className="profile-leaderboard">
                    <h4>Leader board</h4>
                    {leaderboard.length > 0 && userRank !== null && (
                        <>
                            <p className={leaderboard[0].username === userData.name ? "you" : ""}>
                                1. {leaderboard[0].username === userData.name ? "You" : leaderboard[0].username} - {leaderboard[0].score} points
                            </p>
                            {userRank !== 1 && (
                                <>
                                    {userRank === 2 && (
                                        <p className="you">
                                            2. You - {userScore} points
                                        </p>
                                    )}
                                    {userRank > 2 && (
                                        <>
                                            <p>...</p>
                                            <p className="you">
                                                {userRank}. You - {userScore} points
                                            </p>
                                        </>
                                    )}
                                </>
                            )}
                        </>
                    )}
                </div>



                <div className="profile-form">
                    <div className="form-group">
                        <label htmlFor="changedName">Name</label>
                        <input id="changedName" name="changedName" type="text" placeholder="Change your name"  onChange={handleChange} autoComplete="off" readOnly
                            onFocus={(e) => e.target.removeAttribute('readOnly')} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="changedPassword">Password</label>
                        <input id="changedPassword" name="changedPassword" type="password" placeholder="Change password" onChange={handleChange} autoComplete="off" readOnly
                            onFocus={(e) => e.target.removeAttribute('readOnly')} />
                    </div>
                    <button className="save-button" onClick={changeProfile}>Save changes</button>
                    <div className="two-buttons">
                        <button className="logout-button" onClick={logOut}>Log out</button>
                        <button className="delete-button" onClick={deleteProfile}>Delete account</button>
                    </div>
                </div>
            </div>
            <Footer />
        </div>
        : <Navigate to="/login"></Navigate>
    }
    </>
    );
}
export default ProfilePage;