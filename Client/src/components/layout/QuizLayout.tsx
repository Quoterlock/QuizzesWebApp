import { ReactNode } from "react";
import NavBar from "./NavBar";

interface Props {
    children: ReactNode
}

export function QuizLayout({children}: Props) {
    const currentUsername = localStorage.getItem("current-username")
    console.log("Layout updated")
    return(
        <div className="bg-color">
            <NavBar username={currentUsername as string}/>
            <div className="mt-5">
                {children}
            </div>
        </div>
    )
}
