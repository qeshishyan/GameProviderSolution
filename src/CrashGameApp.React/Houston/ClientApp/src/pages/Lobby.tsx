import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'

import { ApplicationBackground } from '../components/ApplicationBackground/ApplicationBackground'
import { ProgressBar } from '../components/ProgressBar/ProgressBar'
import { RocketAnimated } from '../components/RocketAnimated/RocketAnimated'

const Lobby = () => {
  const [isRocketAction, setIsRocketAction] = useState(false)
  const [isLoading, setIsLoading] = useState(false)

  const navigate = useNavigate()
  const onAnimationEnd = () => {
    if (!isRocketAction) return
    navigate('/game')
  }

  return (
    <ApplicationBackground isLobby={true}>
      <div className="rocket-lobby-wrap">
        <RocketAnimated
          isAction={isRocketAction}
          handleClick={() => {
            setIsLoading(true)
          }}
          onLaunchAnimationEnd={() => {
            onAnimationEnd()
          }}
          isLobby={true}
        />
      </div>
      <div className="progress-lobby-wrapper">
        <ProgressBar
          isLoading={isLoading}
          animationTime={3}
          onEnd={() => {
            setIsRocketAction(true)
          }}
        />
      </div>
    </ApplicationBackground>
  )
}

export default Lobby
