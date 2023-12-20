import { ReactNode, useState } from "react";

interface Props {
    children: ReactNode
}

export function QuizLayout({children}: Props) {
    return(
        <div className="bg-color">
            {children}
        </div>
    )
}