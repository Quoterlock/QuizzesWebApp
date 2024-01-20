import { Link } from "react-router-dom";

interface Props {
    username:string
}

export default function NavBar({username}:Props) {
    return(<div className="custom-navbar">
        <div className="col-4">

        </div>
        <div className="col-4">
            <Link to="/" className="styless-link">
                <h5 className="text-center">QuizIt</h5>
            </Link>
        </div>
        {
            (username === null || username === "")
            ? <div className="col-4 d-flex justify-content-end align-items-center">
                <Link to="/login" className="btn active-btn">Login</Link>
            </div>
            : <div className="col-4 d-flex justify-content-end align-items-center">
                <p className="me-2 my-0">{username}</p>
                <Link to="/profile" className="btn minor-btn">
                    <img className="icon-img" src="./src/assets/icons/account-icon.png"/>
                </Link>
            </div>
        }
    </div>)
}