using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using RDEngine.Engine;
using RDEngine.Engine.Animation;
using RDEngine.Engine.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RDEngine.GameScripts
{
    public class TalkingTable : GComponent
    {
        private TextObject _dialogueObject;
        private Animator _anim, _dialogueAnim;
        private string _dialogue;

        private WorldObject _arrow, _player;

        private int _radius;

        private SoundEffect _talk1, _talk2, _talk3;

        private Random _rnd;

        private Door _door;

        private float _doorTime;
        private bool _openDoor, _openedDoor;

        public TalkingTable(TextObject dialogueObject, string dialogue, int radius, Door door = null)
        {
            _radius = radius;

            _dialogueObject = dialogueObject;
            _dialogueAnim = _dialogueObject.GetComponent<Animator>();
            _dialogue = dialogue;

            _talk1 = ContentStorer.SFX["Talk1"];
            _talk2= ContentStorer.SFX["Talk2"];
            _talk3 = ContentStorer.SFX["Talk3"];

            _rnd = new Random();

            _door = door;
            _doorTime = 0;
            _openDoor = _openedDoor = false;
        }

        public override void Start()
        {
            _player = Parent.Scene.FindWithTag("Player") as WorldObject;

            _anim = new Animator(new Dictionary<string, Animation>()
            {
                {
                    "fadeout", new Animation(false, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(500, 150),
                            new Tuple<int, int>(0, 0)
                        }, 0)
                    })
                },
                {
                    "fadein", new Animation(false, new AnimLayer[]
                    {
                        new AnimLayer(new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(500, 0),
                            new Tuple<int, int>(0, 150)
                        }, 0)
                    })
                },
            }, "fadeout", ints: new int[1]);
            Parent.AddComponent(_anim);

            _arrow = new WorldObject("Arrow" + Parent.Tag, ContentStorer.Textures["TalkIndicator"], Vector2.Zero)
            {
                Position = new Vector2(0f, -10f),
                LayerDepth = 0.9f
            };
            _arrow.SetParent(Parent);
            Parent.Scene.AddGameObject(_arrow);
        }

        public override void Update()
        {
            _arrow.Color = new Color(1, 1, 1, 1) * _anim.Ints[0];

            if (Vector2.Distance(Parent.Position, _player.Position) <= _radius)
            {
                if (_anim.GetAnimName() != "fadein")
                    _anim.SetAnimation("fadein");

                if (Input.GetKey(Microsoft.Xna.Framework.Input.Keys.E, KeyGate.Down))
                {
                    _dialogueObject.Text = _dialogue;
                    _dialogueObject.CCPosition = new Vector2(0f, _dialogueObject.CCPosition.Y);
                    _dialogueAnim.SetAnimation("fade");

                    int randNum = _rnd.Next(0, 3);
                    SoundEffect sound = _talk1;
                    switch (randNum)
                    {
                        case 0:
                            sound = _talk1;
                            break;
                        case 1:
                            sound = _talk2;
                            break;
                        case 2:
                            sound = _talk3;
                            break;
                    }
                    sound.Play(0.5f, -0.25f, 0f);

                    _openDoor = true;
                }
            }
            else
            {
                if (_anim.GetAnimName() != "fadeout")
                    _anim.SetAnimation("fadeout");
            }

            if (_openDoor && !_openedDoor)
            {
                _doorTime += Time.DeltaTime;

                if (_doorTime >= 4f)
                {
                    if (_door != null)
                        _door.Open();
                    _openedDoor = true;
                }
            }
        }
    }
}
